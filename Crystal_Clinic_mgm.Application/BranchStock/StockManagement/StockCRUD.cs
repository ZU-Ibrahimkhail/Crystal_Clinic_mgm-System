using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.StockManagement
{
    #region Create Stock
    public class CreateStockCommand : IRequest<int>
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public int? SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public string BarCode { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }

    public class CreateStockHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateStockCommand, int>
    {
        public async Task<int> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var item = context.Items.FirstOrDefault(x => x.ItemId == request.ItemId);
            if (item == null || item.IsDeleted)
            {
                throw new KeyNotFoundException($"Item with Id {request.ItemId} not found!");
            }

            var stock = new Stock
            {
                itemId = request.ItemId,
                quantity = request.Quantity,
                purchasePrice = request.PurchasePrice,
                sellPrice = request.SellPrice,
                purchaseDate = request.PurchaseDate,
                batchNumber = request.BatchNumber,
                SupplierId = request.SupplierId,
                barCode = request.BarCode,
                expiryDate = request.ExpiryDate,
                BranchId = item.BranchId,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now
            };

            context.Stocks.Add(stock);
            item.CurrentStock += request.Quantity;
            item.UseableStock += request.Quantity;

            await context.SaveChangesAsync(cancellationToken);
            return stock.stockId;
        }
    }
    #endregion

    #region Update Stock
    public class UpdateStockCommand : IRequest<int>
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public string BarCode { get; set; } = string.Empty;
        public int? SupplierId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }


    public class UpdateStockHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateStockCommand, int>
    {
        public async Task<int> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return 0;
            var item = context.Items.FirstOrDefault(x => x.ItemId == stock.itemId);
            if (item == null || item.IsDeleted) return 0;

            if (stock.quantity != request.Quantity)
            {
                item.CurrentStock -= stock.quantity;
                item.UseableStock -= stock.quantity;

                item.CurrentStock += request.Quantity;
                item.UseableStock += request.Quantity;
                context.Items.Update(item);
            }
            stock.quantity = request.Quantity;
            stock.purchasePrice = request.PurchasePrice;
            stock.sellPrice = request.SellPrice;
            stock.SupplierId = request.SupplierId;
            stock.purchaseDate = request.PurchaseDate;
            stock.batchNumber = request.BatchNumber;
            stock.barCode = request.BarCode;
            stock.expiryDate = request.ExpiryDate;
            stock.ModifiedBy = loggedInUser.Id;
            stock.ModifiedOn = DateTime.Now;

            context.Stocks.Update(stock);

            await context.SaveChangesAsync(cancellationToken);

            return stock.stockId;
        }
    }
    #endregion

    #region Delete Stock
    public class DeleteStockCommand : IRequest<bool>
    {
        public int StockId { get; set; }
    }

    public class DeleteStockHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteStockCommand, bool>
    {
        public async Task<bool> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await context.Stocks
                .FirstOrDefaultAsync(s => s.stockId == request.StockId && !s.IsDeleted, cancellationToken);
            if (stock == null) return false;
            var item = context.Items.FirstOrDefault(x => x.ItemId == stock.itemId);
            if (item == null || item.IsDeleted) return false;

            item.CurrentStock -= stock.quantity;
            item.UseableStock -= stock.quantity;
            context.Items.Update(item);

            stock.IsDeleted = true;
            stock.ModifiedOn = DateTime.Now;
            stock.ModifiedBy = loggedInUser.Id;

            context.Stocks.Update(stock);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    #region Adjust Stock Quantity

    public class AdjustStockQuantityCommand : IRequest<bool>
    {
        public int StockId { get; set; }
        public int QuantityAdjustment { get; set; }
    }

    public class AdjustStockQuantityHandler(ERP_DbContext context) : IRequestHandler<AdjustStockQuantityCommand, bool>
    {
        public async Task<bool> Handle(AdjustStockQuantityCommand request, CancellationToken cancellationToken)
        {
            var stock = await context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return false;

            stock.quantity += request.QuantityAdjustment;

            // Ensure stock is not negative
            if (stock.quantity < 0)
            {
                return false;
            }
            var item = context.Items.FirstOrDefault(x => x.ItemId == stock.itemId);
            if (item == null || item.IsDeleted) return false;

            item.CurrentStock -= request.QuantityAdjustment;
            item.UseableStock -= request.QuantityAdjustment;
            context.Items.Update(item);

            context.Stocks.Update(stock);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
    #endregion

    #region Update Stock Prices

    public class UpdateStockPricesCommand : IRequest<bool>
    {
        public int StockId { get; set; }
        public decimal NewPurchasePrice { get; set; }
        public decimal NewSellPrice { get; set; }
    }

    public class UpdateStockPricesHandler(ERP_DbContext context) : IRequestHandler<UpdateStockPricesCommand, bool>
    {
        public async Task<bool> Handle(UpdateStockPricesCommand request, CancellationToken cancellationToken)
        {
            var stock = await context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return false;

            stock.purchasePrice = request.NewPurchasePrice;
            stock.sellPrice = request.NewSellPrice;

            context.Stocks.Update(stock);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
    #endregion
}
