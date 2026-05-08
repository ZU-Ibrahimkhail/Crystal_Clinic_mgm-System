using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
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
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        if (invoice.Status != SalesStatus.Draft)
                            return Result.Fail("Only draft invoices can be issued.");

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        invoice.Status = SalesStatus.Issued;
                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;
                        context.SalesInvoices.Update(invoice);

                        var ar = new AccountsReceivable
                        {
                            InvoiceNumber = invoice.InvoiceNumber,
                            CustomerId = invoice.CustomerId,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate ?? invoice.InvoiceDate.AddDays(30),
                            InvoiceAmount = invoice.NetAmount,
                            BalanceAmount = invoice.NetAmount,
                            PaidAmount = 0,
                            Status = ARStatus.Open,
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            BranchId = invoice.BranchId,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            CurrencyRate = 1,
                            Description = $"AR for Sales Invoice {invoice.InvoiceNumber}",
                            Reference = invoice.InvoiceNumber,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };
                        context.AccountsReceivables.Add(ar);

                        var je = new JournalEntry
                        {
                            EntryNumber = LedgerPostingService.GenerateJournalEntryNumber(),
                            EntryDate = invoice.InvoiceDate,
                            Description = $"Sales Invoice issued: {invoice.InvoiceNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = invoice.InvoiceNumber,
                            ReferenceType = "SalesInvoice",
                            BranchId = invoice.BranchId,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            Description = $"DR Accounts Receivable - {invoice.InvoiceNumber}",
                            DebitAmount = invoice.NetAmount,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = invoice.NetAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.SalesRevenueAccountId,
                            Description = $"CR Sales Revenue - {invoice.InvoiceNumber}",
                            DebitAmount = 0,
                            CreditAmount = invoice.NetAmount,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = invoice.NetAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);
                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success("Sales Invoice issued successfully.");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
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
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
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

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        var receiptNumber = GenerateReceiptNumber();
                        var receipt = new SalesReceipt
                        {
                            SalesInvoiceId = request.Dto.SalesInvoiceId,
                            ReceiptNumber = receiptNumber,
                            CustomerId = invoice.CustomerId,
                            AmountReceived = request.Dto.AmountReceived,
                            PaymentMethod = request.Dto.PaymentMethod,
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

                        var ar = await context.AccountsReceivables
                            .FirstOrDefaultAsync(a => a.Reference == invoice.InvoiceNumber && !a.IsDeleted, cancellationToken);

                        if (ar != null)
                        {
                            ar.PaidAmount += request.Dto.AmountReceived;
                            ar.BalanceAmount -= request.Dto.AmountReceived;
                            ar.Status = ar.BalanceAmount <= 0 ? ARStatus.Paid : ARStatus.PartiallyPaid;
                            ar.ModifiedBy = loggedInUser.Id;
                            ar.ModifiedOn = DateTime.UtcNow;
                            context.AccountsReceivables.Update(ar);
                        }

                        var cashAccountId = request.Dto.PaymentMethod == PaymentMethod.BankTransfer
                            ? companyProfile.BankAccountId
                            : companyProfile.CashAccountId;

                        var je = new JournalEntry
                        {
                            EntryNumber = LedgerPostingService.GenerateJournalEntryNumber(),
                            EntryDate = request.Dto.ReceiptDate,
                            Description = $"Sales Receipt recorded: {receiptNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = receiptNumber,
                            ReferenceType = "SalesReceipt",
                            BranchId = invoice.BranchId,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = cashAccountId,
                            Description = $"DR Cash/Bank - {receiptNumber}",
                            DebitAmount = request.Dto.AmountReceived,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.Dto.AmountReceived,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            Description = $"CR Accounts Receivable - {receiptNumber}",
                            DebitAmount = 0,
                            CreditAmount = request.Dto.AmountReceived,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.Dto.AmountReceived,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);
                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success(receipt.Id, "Sales Receipt recorded successfully.");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
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
