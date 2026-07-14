using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
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
                var barCodes = request.Dto.Lines
                    .Where(l => !string.IsNullOrEmpty(l.BarCode))
                    .Select(l => l.BarCode)
                    .ToList();

                if (barCodes.Any())
                {
                    var duplicateBarCode = await context.Stocks
                        .Where(s => barCodes.Contains(s.BarCode))
                        .Select(s => s.BarCode)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (!string.IsNullOrEmpty(duplicateBarCode))
                    {
                        return Result.Fail($"Barcode '{duplicateBarCode}' already exists. Barcode must be unique.");
                    }
                }

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
                    Status = POStatus.Draft,
                    Attachment = request.Dto.Attachment,
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
                            LineTotal = lineDto.Quantity * lineDto.UnitPrice,
                            ItemExpiry = lineDto.ItemExpiry,
                            BarCode = lineDto.BarCode,
                            BatchNumber = lineDto.BatchNumber,
                            ExpectedSalePrice = lineDto.ExpectedSalePrice,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.Now
                        };
                        po.Lines.Add(line);
                    }

                    po.TotalAmount = po.Lines.Sum(l => l.LineTotal);
                    context.Update(po);
                    await context.SaveChangesAsync(cancellationToken);

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

    public class UpdatePurchaseOrderCommandHandler
        (ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdatePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await context.PurchaseOrders
                    .Include(p => p.Lines)
                    .FirstOrDefaultAsync(p => p.Id == request.Dto.Id && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail($"Purchase Order with ID {request.Dto.Id} not found.");

                if (po.Status == POStatus.Cancelled)
                    return Result.Fail($"Cannot update cancelled Purchase Order. PO# {po.PONumber} is in {po.Status} status.");

                if (po.Status == POStatus.Received)
                    return Result.Fail($"Cannot update received Purchase Order. PO# {po.PONumber} is in {po.Status} status. A bill has been created for this PO.");

                if (po.Status != POStatus.Draft && po.Status != POStatus.Open)
                    return Result.Fail($"Cannot update Purchase Order with status '{po.Status}'. Only Draft and Open POs can be updated.");

                if (request.Dto.OrderDate > request.Dto.ExpectedDeliveryDate)
                    return Result.Fail("Order date cannot be after expected delivery date.");

                if (request.Dto.OrderDate < DateTime.Now.Date)
                    return Result.Fail("Order date cannot be in the past.");

                po.OrderDate = request.Dto.OrderDate;
                po.ExpectedDeliveryDate = request.Dto.ExpectedDeliveryDate;
                po.ModifiedBy = loggedInUser.Id;
                po.ModifiedOn = DateTime.UtcNow;

                context.PurchaseOrders.Update(po);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success($"Purchase Order {po.PONumber} updated successfully.");
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
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var po = await context.PurchaseOrders
                            .Include(p => p.Lines)
                            .FirstOrDefaultAsync(p => p.Id == request.PurchaseOrderId && !p.IsDeleted, cancellationToken);

                        if (po == null)
                            return Result.Fail("Purchase Order not found.");

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        // Load items to check if they are fixed assets
                        var itemIds = po.Lines.Select(l => l.ItemId).Distinct().ToList();
                        var items = await context.Items
                            .Where(i => itemIds.Contains(i.ItemId))
                            .ToDictionaryAsync(i => i.ItemId, cancellationToken);

                        // Separate lines by type
                        var inventoryLines = po.Lines.Where(l => !items.ContainsKey(l.ItemId) || !items[l.ItemId].IsFixedAsset).ToList();
                        var fixedAssetLines = po.Lines.Where(l => items.ContainsKey(l.ItemId) && items[l.ItemId].IsFixedAsset).ToList();

                        var inventoryTotal = inventoryLines.Sum(l => l.LineTotal);
                        var fixedAssetTotal = fixedAssetLines.Sum(l => l.LineTotal);

                        po.Status = POStatus.Received;
                        po.ModifiedBy = loggedInUser.Id;
                        po.ModifiedOn = DateTime.UtcNow;

                        var apInvoiceNumber = GenerateInvoiceNumber();
                        var aP = new AccountsPayable
                        {
                            InvoiceNumber = apInvoiceNumber,
                            VendorId = po.VendorId,
                            PurchaseOrderId = po.Id,
                            InvoiceDate = DateTime.UtcNow,
                            DueDate = DateTime.UtcNow.AddDays(30),
                            InvoiceAmount = po.TotalAmount,
                            BalanceAmount = po.TotalAmount,
                            Status = APStatus.Pending,
                            Type = APType.Purchase,
                            ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            CurrencyRate = 1,
                            Description = $"AP created from PO #{po.PONumber}",
                            Reference = po.PONumber,
                            BranchId = po.BranchId,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.AccountsPayables.Add(aP);
                        context.PurchaseOrders.Update(po);

                        // Create Stock records only for inventory items (NOT for fixed assets)
                        if (inventoryLines.Any())
                        {
                            foreach (var line in inventoryLines)
                            {
                                var stock = new Stock
                                {
                                    ItemId = line.ItemId,
                                    Quantity = (int)line.Quantity,
                                    QuantityRemaining = (int)line.Quantity,
                                    SupplierId = po.VendorId,
                                    BranchId = po.BranchId,
                                    PurchasePrice = line.UnitPrice,
                                    SellPrice = line.ExpectedSalePrice ?? 0,
                                    PurchaseDate = DateTime.UtcNow,
                                    ExpiryDate = line.ItemExpiry ?? DateTime.MaxValue,
                                    BarCode = line.BarCode,
                                    BatchNumber = line.BatchNumber,
                                    PurchaseOrderId = po.Id,
                                    IsExpired = false,
                                    CreatedBy = loggedInUser.Id,
                                    CreatedOn = DateTime.UtcNow
                                };

                                context.Stocks.Add(stock);
                            }

                            await context.SaveChangesAsync(cancellationToken);
                        }

                        await context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success("Purchase Order marked as received. Stock records and AP created. GL entry will be created when Vendor Bill is issued.");
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
                return Result.Fail($"Error receiving Purchase Order: {ex.Message}");
            }
        }
        private string GenerateInvoiceNumber()
        {
            return $"AP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
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
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync();
                    try
                    {
                        var vendor = await context.Supplier
                            .FirstOrDefaultAsync(v => v.Id == request.Dto.VendorId, cancellationToken);

                        if (vendor == null)
                            throw new Exception("Vendor not found.");

                        if (request.Dto.PurchaseOrderId.HasValue)
                        {
                            var po = await context.PurchaseOrders
                                .FirstOrDefaultAsync(p => p.Id == request.Dto.PurchaseOrderId, cancellationToken);

                            if (po == null)
                                throw new Exception("Purchase Order not found.");

                            if (po.Status != POStatus.Received)
                                throw new Exception("Purchase Order must be received before billing.");

                            var existingBillsTotal = await context.VendorBills
                                .Where(b => b.PurchaseOrderId == request.Dto.PurchaseOrderId && !b.IsDeleted)
                                .SumAsync(b => b.TotalAmount, cancellationToken);

                            decimal totalBillsAfterCreate = existingBillsTotal + request.Dto.TotalAmount;

                            if (totalBillsAfterCreate > po.TotalAmount)
                            {
                                decimal allowedAmount = po.TotalAmount - existingBillsTotal;
                                throw new Exception($"Bill amount exceeds Purchase Order limit. PO Total: {po.TotalAmount}, Already Billed: {existingBillsTotal}, Allowed Amount: {allowedAmount}, Requested: {request.Dto.TotalAmount}");
                            }
                        }

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

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        // Try to reuse AP from PO Receive (Bill-Driven Accounting)
                        AccountsPayable ap = null;
                        if (bill.PurchaseOrderId.HasValue)
                        {
                            ap = await context.AccountsPayables
                                .FirstOrDefaultAsync(a => a.PurchaseOrderId == bill.PurchaseOrderId && 
                                                         a.VendorBillId == null && 
                                                         !a.IsDeleted, cancellationToken);
                        }

                        // If no existing AP from PO, create a new one (for standalone bills)
                        if (ap == null)
                        {
                            ap = new AccountsPayable
                            {
                                VendorBill = bill,
                                InvoiceNumber = billNumber,
                                Reference = bill.BillNumber,
                                PurchaseOrderId = bill.PurchaseOrderId,
                                VendorId = bill.VendorId,
                                InvoiceDate = bill.BillDate,
                                DueDate = bill.DueDate,
                                InvoiceAmount = bill.TotalAmount,
                                PaidAmount = 0,
                                CurrencyId = companyProfile.BaseCurrencyId,
                                BalanceAmount = bill.TotalAmount,
                                Status = APStatus.Pending,
                                BranchId = bill.BranchId,
                                Description = $"AP created for Vendor Bill {bill.BillNumber}",
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            };
                            context.AccountsPayables.Add(ap);
                        }
                        else
                        {
                            // Reuse existing AP, link it to bill, update amounts
                            ap.VendorBill = bill;
                            ap.InvoiceNumber = billNumber;
                            ap.Reference = bill.BillNumber;
                            ap.InvoiceDate = bill.BillDate;
                            ap.DueDate = bill.DueDate;
                            ap.InvoiceAmount = bill.TotalAmount;
                            ap.BalanceAmount = bill.TotalAmount;
                            ap.Status = APStatus.Pending;
                            ap.Description = $"AP for Vendor Bill {bill.BillNumber}";
                            ap.ModifiedBy = loggedInUser.Id;
                            ap.ModifiedOn = DateTime.UtcNow;
                            context.AccountsPayables.Update(ap);
                        }

                        // Load PO lines to determine if bill is for inventory or fixed assets
                        var inventoryTotal = bill.TotalAmount;
                        var fixedAssetTotal = 0m;

                        if (bill.PurchaseOrderId.HasValue)
                        {
                            var poLines = await context.POLines
                                .Where(l => l.PurchaseOrderId == bill.PurchaseOrderId)
                                .Select(l => new { l.ItemId, l.LineTotal })
                                .ToListAsync(cancellationToken);

                            if (poLines.Any())
                            {
                                var itemIds = poLines.Select(l => l.ItemId).Distinct().ToList();
                                var items = await context.Items
                                    .Where(i => itemIds.Contains(i.ItemId))
                                    .ToDictionaryAsync(i => i.ItemId, cancellationToken);

                                // Calculate totals by type (proportional to bill amount if partial billing)
                                var inventoryLineTotal = poLines
                                    .Where(l => !items.ContainsKey(l.ItemId) || !items[l.ItemId].IsFixedAsset)
                                    .Sum(l => l.LineTotal);

                                var fixedAssetLineTotal = poLines
                                    .Where(l => items.ContainsKey(l.ItemId) && items[l.ItemId].IsFixedAsset)
                                    .Sum(l => l.LineTotal);

                                if (inventoryLineTotal + fixedAssetLineTotal > 0)
                                {
                                    // Allocate bill amount proportionally
                                    decimal allocationRatio = bill.TotalAmount / (inventoryLineTotal + fixedAssetLineTotal);
                                    inventoryTotal = inventoryLineTotal * allocationRatio;
                                    fixedAssetTotal = fixedAssetLineTotal * allocationRatio;
                                }
                            }
                        }

                        var je = new JournalEntry
                        {
                            EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                            EntryDate = bill.BillDate,
                            Description = $"Journal Entry for Vendor Bill {bill.BillNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = bill.BillNumber,
                            ReferenceType = "VendorBill",
                            BranchId = bill.BranchId,
                            ReceiptId = bill.Id,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        // Add journal entry for inventory items
                        if (inventoryTotal > 0)
                        {
                            je.JournalEntryLines.Add(new JournalEntryLine
                            {
                                ChartOfAccountId = companyProfile.PurchaseExpenseAccountId,
                                Description = $"DR Purchase/Inventory - Vendor Bill {bill.BillNumber}",
                                DebitAmount = inventoryTotal,
                                Status = JournalEntryStatus.Posted,
                                CreditAmount = 0,
                                CurrencyId = companyProfile.BaseCurrencyId,
                                ExchangeRate = 1,
                                ReceiptId = bill.Id,
                                AmountInBaseCurrency = inventoryTotal,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            });

                            je.JournalEntryLines.Add(new JournalEntryLine
                            {
                                ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                                Description = $"CR Accounts Payable - Inventory - Vendor Bill {bill.BillNumber}",
                                DebitAmount = 0,
                                Status = JournalEntryStatus.Posted,
                                CreditAmount = inventoryTotal,
                                CurrencyId = companyProfile.BaseCurrencyId,
                                ExchangeRate = 1,
                                ReceiptId = bill.Id,
                                AmountInBaseCurrency = inventoryTotal,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            });
                        }

                        // Add journal entry for fixed assets
                        if (fixedAssetTotal > 0 && companyProfile.FixedAssetAccountId.HasValue)
                        {
                            je.JournalEntryLines.Add(new JournalEntryLine
                            {
                                ChartOfAccountId = companyProfile.FixedAssetAccountId.Value,
                                Description = $"DR Fixed Asset - Vendor Bill {bill.BillNumber}",
                                DebitAmount = fixedAssetTotal,
                                CreditAmount = 0,
                                CurrencyId = companyProfile.BaseCurrencyId,
                                ExchangeRate = 1,
                                Status = JournalEntryStatus.Posted,
                                ReceiptId = bill.Id,
                                AmountInBaseCurrency = fixedAssetTotal,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            });

                            je.JournalEntryLines.Add(new JournalEntryLine
                            {
                                ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                                Description = $"CR Accounts Payable - Fixed Asset - Vendor Bill {bill.BillNumber}",
                                DebitAmount = 0,
                                CreditAmount = fixedAssetTotal,
                                CurrencyId = companyProfile.BaseCurrencyId,
                                ExchangeRate = 1,
                                Status = JournalEntryStatus.Posted,
                                ReceiptId = bill.Id,
                                AmountInBaseCurrency = fixedAssetTotal,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            });
                        }

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new Exception("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);

                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return Result.Success(bill.Id, $"Vendor Bill {billNumber} created successfully.");
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
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var bill = await context.VendorBills
                            .FirstOrDefaultAsync(b => b.Id == request.Dto.Id && !b.IsDeleted, cancellationToken);

                        if (bill == null)
                            return Result.Fail("Vendor Bill not found.");

                        if (bill.Status != BillStatus.Unpaid)
                            return Result.Fail("Only unpaid bills can be updated.");


                        var ap = await context.AccountsPayables
                            .FirstOrDefaultAsync(a => a.VendorBillId == bill.Id && !a.IsDeleted, cancellationToken);

                        if (ap != null && ap.PaidAmount > 0)
                            return Result.Fail("Cannot update bill after payment has been made.");

                        if (bill.PurchaseOrderId.HasValue)
                        {
                            var po = await context.PurchaseOrders
                                .FirstOrDefaultAsync(p => p.Id == bill.PurchaseOrderId, cancellationToken);

                            if (po != null)
                            {
                                var otherBillsTotal = await context.VendorBills
                                    .Where(b => b.PurchaseOrderId == bill.PurchaseOrderId && 
                                               b.Id != bill.Id && 
                                               !b.IsDeleted)
                                    .SumAsync(b => b.TotalAmount, cancellationToken);

                                decimal totalBillsAfterUpdate = otherBillsTotal + request.Dto.TotalAmount;

                                if (totalBillsAfterUpdate > po.TotalAmount)
                                {
                                    decimal allowedAmount = po.TotalAmount - otherBillsTotal;
                                    return Result.Fail($"Bill amount exceeds Purchase Order limit. PO Total: {po.TotalAmount}, Other Bills Total: {otherBillsTotal}, Allowed Amount: {allowedAmount}, Requested: {request.Dto.TotalAmount}");
                                }
                            }
                        }

                        bill.BillDate = request.Dto.BillDate;
                        bill.DueDate = request.Dto.DueDate;
                        bill.TotalAmount = request.Dto.TotalAmount;
                        bill.ModifiedBy = loggedInUser.Id;
                        bill.ModifiedOn = DateTime.UtcNow;

                        context.VendorBills.Update(bill);

                        if (ap != null)
                        {
                            ap.InvoiceAmount = bill.TotalAmount;
                            ap.BalanceAmount = ap.InvoiceAmount - ap.PaidAmount;
                            ap.InvoiceDate = bill.BillDate;
                            ap.DueDate = bill.DueDate;
                        }

                        var existingJe = await context.JournalEntries
                             .Include(j => j.JournalEntryLines)
                             .Where(j =>
                                 j.ReferenceNumber == bill.BillNumber &&
                                 j.ReferenceType == "VendorBill" &&
                                 j.Status == JournalEntryStatus.Posted)
                             .OrderByDescending(j => j.Id)
                             .FirstOrDefaultAsync(cancellationToken);

                        if (existingJe != null)
                        {
                            existingJe.Status = JournalEntryStatus.Voided;
                            existingJe.ModifiedBy = loggedInUser.Id;
                            existingJe.ModifiedOn = DateTime.UtcNow;

                            var reversalJe = new JournalEntry
                            {
                                EntryNumber = $"REV-{existingJe.EntryNumber}",
                                EntryDate = DateTime.UtcNow,
                                Description = $"Reversal of {existingJe.EntryNumber}",
                                Status = JournalEntryStatus.Posted,
                                ReferenceNumber = bill.BillNumber,
                                ReferenceType = "VendorBill-Reversal",
                                BranchId = bill.BranchId,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            };

                            foreach (var line in existingJe.JournalEntryLines)
                            {
                                reversalJe.JournalEntryLines.Add(new JournalEntryLine
                                {
                                    ChartOfAccountId = line.ChartOfAccountId,
                                    Description = "Reversal entry",
                                    DebitAmount = line.CreditAmount,   // swapped
                                    CreditAmount = line.DebitAmount,   // swapped
                                    CurrencyId = line.CurrencyId,
                                    Status = JournalEntryStatus.Posted,
                                    CreatedBy = loggedInUser.Id,
                                    CreatedOn = DateTime.UtcNow
                                });
                            }
                            context.JournalEntries.Add(reversalJe);
                            await LedgerPostingService.PostToGeneralLedgerAsync(context, reversalJe, cancellationToken);


                        }


                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            throw new Exception("Company profile not configured");

                        var newJe = new JournalEntry
                        {
                            EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                            EntryDate = bill.BillDate,
                            Description = $"Updated Vendor Bill {bill.BillNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = bill.BillNumber,
                            ReferenceType = "VendorBill",
                            BranchId = bill.BranchId,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        // Debit Expense
                        newJe.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.PurchaseExpenseAccountId,
                            Description = $"Debit for updated Vendor Bill {bill.BillNumber}",
                            DebitAmount = bill.TotalAmount,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        // Credit AP
                        newJe.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                            Description = $"Credit for updated Vendor Bill {bill.BillNumber}",
                            DebitAmount = 0,
                            CreditAmount = bill.TotalAmount,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        context.JournalEntries.Add(newJe);

                        await context.SaveChangesAsync(cancellationToken);
                        await LedgerPostingService.PostToGeneralLedgerAsync(context, newJe, cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success("Vendor Bill updated successfully.");
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
                return Result.Fail($"Error updating Vendor Bill: {ex.Message}");
            }

        }
        #endregion

    #region Pay Vendor Bill
        public class PayVendorBillCommand : IRequest<Result>
        {
            public int VendorBillId { get; set; }
            public decimal PaymentAmount { get; set; }
            public int PaymentMethodId { get; set; }
            public string? Reference { get; set; }
        }

        public class PayVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PayVendorBillCommand, Result>
        {
            public async Task<Result> Handle(PayVendorBillCommand request, CancellationToken cancellationToken)
            {
                var strategy = context.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                    try
                    {
                        var ap = await context.AccountsPayables
                            .FirstOrDefaultAsync(a => a.VendorBillId == request.VendorBillId && !a.IsDeleted, cancellationToken);

                        if (ap == null)
                            return Result.Fail("Accounts Payable not found.");

                        if (request.PaymentAmount <= 0)
                            return Result.Fail("Invalid payment amount.");

                        if (request.PaymentAmount > ap.BalanceAmount)
                            return Result.Fail("Payment exceeds remaining balance.");

                        if (request.PaymentMethodId < 0)
                            return Result.Fail("Invalid payment method.");

                        var rate = ap.CurrencyRate == 0 ? 1 : (decimal)ap.CurrencyRate;

                        var payment = new Payment
                        {
                            AccountsPayableId = ap.Id,
                            PaymentNumber = GeneratePaymentNumber(),
                            PaymentDate = DateTime.UtcNow,
                            AmountPaid = request.PaymentAmount,
                            PaymentMethodId = request.PaymentMethodId,
                            Reference = request.Reference ?? $"Payment for Bill {ap.InvoiceNumber}",
                            CurrencyId = ap.CurrencyId,
                            ExchangeRate = rate,
                            AmountInBaseCurrency = request.PaymentAmount * rate,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.Payments.Add(payment);

                        ap.PaidAmount += request.PaymentAmount;
                        ap.BalanceAmount = ap.InvoiceAmount - ap.PaidAmount;

                        if (ap.BalanceAmount == 0)
                        {
                            ap.Status = APStatus.Paid;

                            var bill = await context.VendorBills
                                .FirstOrDefaultAsync(b => b.Id == request.VendorBillId && !b.IsDeleted, cancellationToken);

                            if (bill == null)
                                return Result.Fail("Vendor Bill not found.");

                            bill.Status = BillStatus.Paid;
                        }
                        else
                        {
                            ap.Status = APStatus.PartiallyPaid;
                        }

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);

                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        var paymentMethod = (PaymentMethod)request.PaymentMethodId;
                        
                        int cashAccountId;
                        if (paymentMethod == PaymentMethod.Cash)
                        {
                            if (companyProfile.CashAccountId == 0)
                                return Result.Fail("Cash account not configured in company profile.");
                            cashAccountId = companyProfile.CashAccountId;
                        }
                        else if (paymentMethod == PaymentMethod.BankTransfer || 
                                 paymentMethod == PaymentMethod.CreditCard || 
                                 paymentMethod == PaymentMethod.Check)
                        {
                            if (companyProfile.BankAccountId == 0)
                                return Result.Fail("Bank account not configured in company profile.");
                            cashAccountId = companyProfile.BankAccountId;
                        }
                        else
                        {
                            return Result.Fail($"Unsupported payment method: {paymentMethod}");
                        }

                        var je = new JournalEntry
                        {
                            EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                            EntryDate = payment.PaymentDate,
                            Description = payment.Reference,
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = payment.PaymentNumber,
                            ReferenceType = "Vendor Payment",
                            BranchId = ap.BranchId,
                            PaymentId = payment.Id,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                            Description = $"Debited AP for payment {payment.PaymentNumber}",
                            DebitAmount = request.PaymentAmount,
                            CreditAmount = 0,
                            CurrencyId = ap.CurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = rate,
                            PaymentId = payment.Id,
                            AmountInBaseCurrency = request.PaymentAmount * rate,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = cashAccountId,
                            Description = $"Credit for payment {payment.PaymentNumber}",
                            DebitAmount = 0,
                            PaymentId = payment.Id,
                            CreditAmount = request.PaymentAmount,
                            CurrencyId = ap.CurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = rate,
                            AmountInBaseCurrency = request.PaymentAmount * rate,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        context.JournalEntries.Add(je);

                        await context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);

                        return Result.Success("Vendor Bill marked as paid.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result.Fail($"Error processing payment: {ex.Message}");
                    }
                });
            }
            private string GeneratePaymentNumber()
            {
                return $"PMT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }
        }
    }
    #endregion
}
