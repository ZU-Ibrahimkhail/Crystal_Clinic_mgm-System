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
    #region Create Purchase Order
    public class CreatePurchaseOrderCommand : IRequest<Result>
    {
        public CreatePurchaseOrderDto Dto { get; set; } = null!;
    }

    public class CreatePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreatePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vendor = await context.Supplier
                    .FirstOrDefaultAsync(v => v.Id == request.Dto.VendorId, cancellationToken);

                if (vendor == null)
                    return Result.Fail("Vendor not found.");

                var poNumber = GeneratePONumber();
                var po = new PurchaseOrder
                {
                    PONumber = poNumber,
                    VendorId = request.Dto.VendorId,
                    OrderDate = request.Dto.OrderDate,
                    ExpectedDeliveryDate = request.Dto.ExpectedDeliveryDate,
                    BranchId = request.Dto.BranchId,
                    Status = POStatus.Open,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };
                context.PurchaseOrders.Add(po);
                await context.SaveChangesAsync(cancellationToken);

                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var line = new POLine
                        {
                            PurchaseOrderId = po.Id,
                            ItemId = lineDto.ItemId,
                            ItemDescription = lineDto.Description,
                            Quantity = lineDto.Quantity,
                            UnitPrice = lineDto.UnitPrice,
                            LineTotal = lineDto.Quantity * lineDto.UnitPrice
                        };
                        po.Lines.Add(line);
                    }

                    po.TotalAmount = po.Lines.Sum(l => l.LineTotal);
                }


                return Result.Success(po.Id, $"Purchase Order {poNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Purchase Order: {ex.Message}");
            }
        }

        private string GeneratePONumber()
        {
            return $"PO-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Update Purchase Order
    public class UpdatePurchaseOrderCommand : IRequest<Result>
    {
        public UpdatePurchaseOrderDto Dto { get; set; } = null!;
    }

    public class UpdatePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdatePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await context.PurchaseOrders
                    .Include(p => p.Lines)
                    .FirstOrDefaultAsync(p => p.Id == request.Dto.Id && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail("Purchase Order not found.");

                if (po.Status != POStatus.Open)
                    return Result.Fail("Only open POs can be updated.");

                po.OrderDate = request.Dto.OrderDate;
                po.ExpectedDeliveryDate = request.Dto.ExpectedDeliveryDate;
                po.ModifiedBy = loggedInUser.Id;
                po.ModifiedOn = DateTime.UtcNow;

                context.PurchaseOrders.Update(po);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Purchase Order updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Purchase Order: {ex.Message}");
            }
        }
    }
    #endregion

    #region Receive Purchase Order
    public class ReceivePurchaseOrderCommand : IRequest<Result>
    {
        public int PurchaseOrderId { get; set; }
    }

    public class ReceivePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ReceivePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await context.PurchaseOrders
                    .FirstOrDefaultAsync(p => p.Id == request.PurchaseOrderId && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail("Purchase Order not found.");

                po.Status = POStatus.Received;
                po.ModifiedBy = loggedInUser.Id;
                po.ModifiedOn = DateTime.UtcNow;

                context.PurchaseOrders.Update(po);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Purchase Order marked as received.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error receiving Purchase Order: {ex.Message}");
            }
        }
    }
    #endregion

    #region Create Vendor Bill
    public class CreateVendorBillCommand : IRequest<Result>
    {
        public CreateVendorBillDto Dto { get; set; } = null!;
    }

    public class CreateVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateVendorBillCommand, Result>
    {
        public async Task<Result> Handle(CreateVendorBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vendor = await context.Supplier
                    .FirstOrDefaultAsync(v => v.Id == request.Dto.VendorId, cancellationToken);

                if (vendor == null)
                    return Result.Fail("Vendor not found.");

                var billNumber = GenerateBillNumber();
                var bill = new VendorBill
                {
                    BillNumber = billNumber,
                    PurchaseOrderId = request.Dto.PurchaseOrderId,
                    VendorId = request.Dto.VendorId,
                    BillDate = request.Dto.BillDate,
                    DueDate = request.Dto.DueDate,
                    TotalAmount = request.Dto.TotalAmount,
                    BranchId = request.Dto.BranchId,
                    Status = BillStatus.Unpaid,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.VendorBills.Add(bill);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(bill.Id, $"Vendor Bill {billNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Vendor Bill: {ex.Message}");
            }
        }

        private string GenerateBillNumber()
        {
            return $"VB-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Update Vendor Bill
    public class UpdateVendorBillCommand : IRequest<Result>
    {
        public UpdateVendorBillDto Dto { get; set; } = null!;
    }

    public class UpdateVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateVendorBillCommand, Result>
    {
        public async Task<Result> Handle(UpdateVendorBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await context.VendorBills
                    .FirstOrDefaultAsync(b => b.Id == request.Dto.Id && !b.IsDeleted, cancellationToken);

                if (bill == null)
                    return Result.Fail("Vendor Bill not found.");

                if (bill.Status != BillStatus.Unpaid)
                    return Result.Fail("Only unpaid bills can be updated.");

                bill.BillDate = request.Dto.BillDate;
                bill.DueDate = request.Dto.DueDate;
                bill.TotalAmount = request.Dto.TotalAmount;
                bill.ModifiedBy = loggedInUser.Id;
                bill.ModifiedOn = DateTime.UtcNow;

                context.VendorBills.Update(bill);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Vendor Bill updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Vendor Bill: {ex.Message}");
            }
        }
    }
    #endregion

    #region Mark Vendor Bill As Paid
    public class MarkVendorBillAsPaidCommand : IRequest<Result>
    {
        public int VendorBillId { get; set; }
    }

    public class MarkVendorBillAsPaidCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<MarkVendorBillAsPaidCommand, Result>
    {
        public async Task<Result> Handle(MarkVendorBillAsPaidCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await context.VendorBills
                    .FirstOrDefaultAsync(b => b.Id == request.VendorBillId && !b.IsDeleted, cancellationToken);

                if (bill == null)
                    return Result.Fail("Vendor Bill not found.");

                bill.Status = BillStatus.Paid;
                bill.ModifiedBy = loggedInUser.Id;
                bill.ModifiedOn = DateTime.UtcNow;

                context.VendorBills.Update(bill);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Vendor Bill marked as paid.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error marking Vendor Bill as paid: {ex.Message}");
            }
        }
    }
    #endregion
}
