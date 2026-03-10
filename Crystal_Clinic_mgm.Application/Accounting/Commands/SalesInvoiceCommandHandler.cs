using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Sales Invoice
    public class CreateSalesInvoiceCommand : IRequest<Result>
    {
        public CreateSalesInvoiceDto Dto { get; set; } = null!;
    }

    public class CreateSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await context.Patient
                    .FirstOrDefaultAsync(p => p.patientId == request.Dto.CustomerId, cancellationToken);

                if (customer == null)
                    return Result.Fail("Customer not found.");

                var invoiceNumber = GenerateInvoiceNumber();
                
                var invoice = new SalesInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    CustomerId = request.Dto.CustomerId,
                    InvoiceDate = request.Dto.InvoiceDate,
                    DueDate = request.Dto.DueDate,
                    SalesArea = request.Dto.SalesArea,
                    BranchId = request.Dto.BranchId,
                    Status = SalesStatus.Draft,
                    Attachment = request.Dto.Attachment,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.SalesInvoices.Add(invoice);
                await context.SaveChangesAsync(cancellationToken);
                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var line = new SalesInvoiceLine
                        {
                            SalesInvoiceId = invoice.Id,
                            ServiceId = lineDto.ServiceId,
                            InventoryItemId = lineDto.InventoryItemId,
                            Description = lineDto.Description,
                            Quantity = lineDto.Quantity,
                            UnitPrice = lineDto.UnitPrice,
                            LineTotal = lineDto.Quantity * lineDto.UnitPrice,
                            DiscountAmount = lineDto.DiscountAmount,
                            TaxAmount = lineDto.TaxAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };
                        invoice.Lines.Add(line);
                    }

                    invoice.TotalAmount = invoice.Lines.Sum(l => l.LineTotal);
                    invoice.DiscountAmount = invoice.Lines.Sum(l => l.DiscountAmount);
                    invoice.TaxAmount = invoice.Lines.Sum(l => l.TaxAmount);
                    invoice.NetAmount = invoice.TotalAmount - invoice.DiscountAmount + invoice.TaxAmount;
                    context.Update(invoice);
                    await context.SaveChangesAsync(cancellationToken);
                }


                return Result.Success(invoice.Id, $"Sales Invoice {invoiceNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Sales Invoice: {ex.Message}");
            }
        }

        private string GenerateInvoiceNumber()
        {
            return $"SI-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Issue Sales Invoice
    public class IssueSalesInvoiceCommand : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
    }

    public class IssueSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<IssueSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(IssueSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await context.SalesInvoices
                    .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                if (invoice == null)
                    return Result.Fail("Sales Invoice not found.");

                if (invoice.Status != SalesStatus.Draft)
                    return Result.Fail("Only draft invoices can be issued.");

                invoice.Status = SalesStatus.Issued;
                invoice.ModifiedBy = loggedInUser.Id;
                invoice.ModifiedOn = DateTime.UtcNow;

                context.SalesInvoices.Update(invoice);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Sales Invoice issued successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error issuing Sales Invoice: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Sales Receipt
    public class RecordSalesReceiptCommand : IRequest<Result>
    {
        public CreateSalesReceiptDto Dto { get; set; } = null!;
    }

    public class RecordSalesReceiptCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordSalesReceiptCommand, Result>
    {
        public async Task<Result> Handle(RecordSalesReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await context.SalesInvoices
                    .Include(i => i.Receipts)
                    .FirstOrDefaultAsync(i => i.Id == request.Dto.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                if (invoice == null)
                    return Result.Fail("Sales Invoice not found.");

                var totalReceived = invoice.Receipts.Sum(r => r.AmountReceived) + request.Dto.AmountReceived;
                if (totalReceived > invoice.NetAmount)
                    return Result.Fail("Receipt amount exceeds invoice total.");

                var receipt = new SalesReceipt
                {
                    SalesInvoiceId = request.Dto.SalesInvoiceId,
                    ReceiptNumber = GenerateReceiptNumber(),
                    CustomerId = invoice.CustomerId,
                    AmountReceived = request.Dto.AmountReceived,
                    PaymentMethodId = request.Dto.PaymentMethodId,
                    ReceiptDate = request.Dto.ReceiptDate,
                    Reference = request.Dto.Reference ?? string.Empty,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                if (totalReceived == invoice.NetAmount)
                    invoice.Status = SalesStatus.Paid;
                else
                    invoice.Status = SalesStatus.Partial;

                invoice.ModifiedBy = loggedInUser.Id;
                invoice.ModifiedOn = DateTime.UtcNow;

                context.SalesReceipts.Add(receipt);
                context.SalesInvoices.Update(invoice);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(receipt.Id, "Sales Receipt recorded successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording Sales Receipt: {ex.Message}");
            }
        }

        private string GenerateReceiptNumber()
        {
            return $"SR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion
}
