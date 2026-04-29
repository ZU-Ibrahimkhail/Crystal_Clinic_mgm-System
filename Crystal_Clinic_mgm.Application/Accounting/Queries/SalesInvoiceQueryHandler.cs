using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Sales Invoices
    public class GetAllSalesInvoicesQuery : IRequest<Result>
    {
        public int? CustomerId { get; set; }
        public SalesStatus? Status { get; set; }
        public int? BranchId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllSalesInvoicesHandler(ERP_DbContext context) : IRequestHandler<GetAllSalesInvoicesQuery, Result>
    {
        public async Task<Result> Handle(GetAllSalesInvoicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.SalesInvoices
                    .Include(i => i.Lines)
                    .Include(i => i.Receipts)
                    .Where(i => !i.IsDeleted);

                if (request.CustomerId.HasValue)
                    query = query.Where(i => i.CustomerId == request.CustomerId.Value);

                if (request.Status.HasValue)
                    query = query.Where(i => i.Status == request.Status.Value);

                if (request.BranchId.HasValue)
                    query = query.Where(i => i.BranchId == request.BranchId.Value);

                var total = await query.CountAsync(cancellationToken);
                var invoices = await query
                    .OrderByDescending(i => i.InvoiceDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = invoices.Select(i => new SalesInvoiceDto
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    CustomerId = i.CustomerId,
                    CustomerName = i.Customer?.name ?? string.Empty,
                    InvoiceDate = i.InvoiceDate,
                    TotalAmount = i.TotalAmount,
                    TaxAmount = i.TaxAmount,
                    DiscountAmount = i.DiscountAmount,
                    NetAmount = i.NetAmount,
                    Status = i.Status,
                    Notes = i.
                    SalesArea = i.SalesArea,
                    BranchId = i.BranchId,
                    Lines = context.SalesInvoiceLines
                        .Where(l => !l.IsDeleted && l.SalesInvoiceId == i.Id)
                        .Select(l => new SalesInvoiceLineDto
                        {
                            InventoryItemId = l.InventoryItemId,
                            ServiceId = l.ServiceId,
                            Description = l.Description,
                            Quantity = l.Quantity,
                            UnitPrice = l.UnitPrice,
                            LineTotal = l.LineTotal,
                            DiscountAmount = l.DiscountAmount,
                            TaxAmount = l.TaxAmount
                        }).ToList()

                }).ToList();

                return Result.Success(new { Data = dtos, Total = total, PageNumber = request.PageNumber, PageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Sales Invoices: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Sales Invoice By Id
    public class GetSalesInvoiceByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetSalesInvoiceByIdHandler(ERP_DbContext context) : IRequestHandler<GetSalesInvoiceByIdQuery, Result>
    {
        public async Task<Result> Handle(GetSalesInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await context.SalesInvoices
                    .Include(i => i.Lines)
                    .Include(i => i.Receipts)
                    .FirstOrDefaultAsync(i => i.Id == request.Id && !i.IsDeleted, cancellationToken);

                if (invoice == null)
                    return Result.Fail("Sales Invoice not found.");

                var validReceipts = invoice.Receipts
                    .Where(r => !r.IsDeleted);

                var paidAmount = validReceipts.Sum(r => r.AmountReceived);

                var balanceAmount = invoice.NetAmount - paidAmount;

                var dto = new SalesInvoiceDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    CustomerId = invoice.CustomerId,
                    CustomerName = invoice.Customer?.name ?? string.Empty,
                    InvoiceDate = invoice.InvoiceDate,
                    TotalAmount = invoice.TotalAmount,
                    TaxAmount = invoice.TaxAmount,
                    DiscountAmount = invoice.DiscountAmount,
                    NetAmount = invoice.NetAmount,
                    BalanceAmount = balanceAmount,
                    Status = invoice.Status,
                    SalesArea = invoice.SalesArea,
                    BranchId = invoice.BranchId,
                    Attachment = invoice.Attachment,
                    Lines = invoice.Lines
                        .Where(l => !l.IsDeleted)
                        .Select(l => new SalesInvoiceLineDto
                        {
                            InventoryItemId = l.InventoryItemId,
                            ServiceId = l.ServiceId,
                            Description = l.Description,
                            Quantity = l.Quantity,
                            UnitPrice = l.UnitPrice,
                            LineTotal = l.LineTotal,
                            DiscountAmount = l.DiscountAmount,
                            TaxAmount = l.TaxAmount
                        }).ToList(),
                    Receipts = invoice.Receipts
                        .Where(r => !r.IsDeleted)
                        .Select(r => new SalesReceiptDto
                        {
                            Id = r.Id,
                            SalesInvoiceId = r.SalesInvoiceId,
                            ReceiptNumber = r.ReceiptNumber,
                            CustomerId = r.CustomerId,
                            CustomerName = r.Customer?.name ?? string.Empty,
                            AmountReceived = r.AmountReceived,
                            PaymentMethod = r.PaymentMethod,
                            ReceiptDate = r.ReceiptDate,
                            Reference = r.Reference
                        }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Sales Invoice: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Sales Receipts By Invoice
    public class GetSalesReceiptsByInvoiceQuery : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
    }

    public class GetSalesReceiptsByInvoiceHandler(ERP_DbContext context) : IRequestHandler<GetSalesReceiptsByInvoiceQuery, Result>
    {
        public async Task<Result> Handle(GetSalesReceiptsByInvoiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var receipts = await context.SalesReceipts
                    .Where(r => r.SalesInvoiceId == request.SalesInvoiceId && !r.IsDeleted)
                    .OrderByDescending(r => r.ReceiptDate)
                    .ToListAsync(cancellationToken);

                var dtos = receipts.Select(r => new SalesReceiptDto
                {
                    Id = r.Id,
                    SalesInvoiceId = r.SalesInvoiceId,
                    ReceiptNumber = r.ReceiptNumber,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.name ?? string.Empty,
                    AmountReceived = r.AmountReceived,
                    PaymentMethod = r.PaymentMethod,
                    ReceiptDate = r.ReceiptDate,
                    Reference = r.Reference
                }).ToList();

                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Sales Receipts: {ex.Message}");
            }
        }
    }
    #endregion
}
