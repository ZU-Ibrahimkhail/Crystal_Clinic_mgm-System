using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Purchase Orders
    public class GetAllPurchaseOrdersQuery : IRequest<Result>
    {
        public int? VendorId { get; set; }
        public POStatus? Status { get; set; }
        public int? BranchId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllPurchaseOrdersHandler(ERP_DbContext context) : IRequestHandler<GetAllPurchaseOrdersQuery, Result>
    {
        public async Task<Result> Handle(GetAllPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.PurchaseOrders
                    .Include(p => p.Lines)
                    .Where(p => !p.IsDeleted);

                if (request.VendorId.HasValue)
                    query = query.Where(p => p.VendorId == request.VendorId.Value);

                if (request.Status.HasValue)
                    query = query.Where(p => p.Status == request.Status.Value);

                if (request.BranchId.HasValue)
                    query = query.Where(p => p.BranchId == request.BranchId.Value);

                var total = await query.CountAsync(cancellationToken);
                var orders = await query
                    .OrderByDescending(p => p.OrderDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = orders.Select(p => new PurchaseOrderDto
                {
                    Id = p.Id,
                    PONumber = p.PONumber,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor?.Name ?? string.Empty,
                    OrderDate = p.OrderDate,
                    ExpectedDeliveryDate = p.ExpectedDeliveryDate,
                    TotalAmount = p.TotalAmount,
                    Status = p.Status,
                    BranchId = p.BranchId
                }).ToList();

                return Result.Success(new { Data = dtos, Total = total, PageNumber = request.PageNumber, PageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Purchase Orders: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Purchase Order By Id
    public class GetPurchaseOrderByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetPurchaseOrderByIdHandler(ERP_DbContext context) : IRequestHandler<GetPurchaseOrderByIdQuery, Result>
    {
        public async Task<Result> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await context.PurchaseOrders
                    .Include(p => p.Lines)
                    .FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail("Purchase Order not found.");

                var dto = new PurchaseOrderDto
                {
                    Id = po.Id,
                    VendorId = po.VendorId,
                    VendorName = po.Vendor?.Name ?? string.Empty,
                    BranchId = po.BranchId,
                    BranchName = po.Branch?.EnglishName,
                    PONumber = po.PONumber,
                    OrderDate = po.OrderDate,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    TotalAmount = po.TotalAmount,
                    Status = po.Status,
                    Attachment = po.Attachment,
                    Lines = po.Lines.Select(l => new POLineDto
                    {
                        Id = l.Id,
                        ItemId = l.ItemId,
                        Description = l.ItemDescription,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice,
                        LineTotal = l.UnitPrice
                    }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Purchase Order: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get All Vendor Bills
    public class GetAllVendorBillsQuery : IRequest<Result>
    {
        public int? VendorId { get; set; }
        public BillStatus? Status { get; set; }
        public int? BranchId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllVendorBillsHandler(ERP_DbContext context) : IRequestHandler<GetAllVendorBillsQuery, Result>
    {
        public async Task<Result> Handle(GetAllVendorBillsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.VendorBills
                    .Where(b => !b.IsDeleted);

                if (request.VendorId.HasValue)
                    query = query.Where(b => b.VendorId == request.VendorId.Value);

                if (request.Status.HasValue)
                    query = query.Where(b => b.Status == request.Status.Value);

                if (request.BranchId.HasValue)
                    query = query.Where(b => b.BranchId == request.BranchId.Value);

                var total = await query.CountAsync(cancellationToken);
                var bills = await query
                    .OrderByDescending(b => b.BillDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = bills.Select(b => new VendorBillDto
                {
                    Id = b.Id,
                    BillNumber = b.BillNumber,
                    PurchaseOrderId = b.PurchaseOrderId,
                    PONumber = b.PurchaseOrder?.PONumber ?? string.Empty,
                    VendorId = b.VendorId,
                    VendorName = b.Vendor?.Name ?? string.Empty,
                    BillDate = b.BillDate,
                    DueDate = b.DueDate,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    BranchId = b.BranchId
                }).ToList();

                return Result.Success(new { Data = dtos, Total = total, PageNumber = request.PageNumber, PageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Vendor Bills: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Vendor Bill By Id
    public class GetVendorBillByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetVendorBillByIdHandler(ERP_DbContext context) : IRequestHandler<GetVendorBillByIdQuery, Result>
    {
        public async Task<Result> Handle(GetVendorBillByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await context.VendorBills
                    .Include(b => b.PurchaseOrder)
                    .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, cancellationToken);

                if (bill == null)
                    return Result.Fail("Vendor Bill not found.");

                var dto = new VendorBillDto
                {
                    Id = bill.Id,
                    BillNumber = bill.BillNumber,
                    PurchaseOrderId = bill.PurchaseOrderId,
                    PONumber = bill.PurchaseOrder?.PONumber ?? string.Empty,
                    VendorId = bill.VendorId,
                    VendorName = bill.Vendor?.Name ?? string.Empty,
                    BillDate = bill.BillDate,
                    DueDate = bill.DueDate,
                    TotalAmount = bill.TotalAmount,
                    Status = bill.Status,
                    BranchId = bill.BranchId
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Vendor Bill: {ex.Message}");
            }
        }
    }
    #endregion
}
