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
        public DateTime PurchaseDate { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public string BarCode { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }

    public class CreateStockHandler : IRequestHandler<CreateStockCommand, int>
    {
        private readonly ERP_DbContext _context;
        public CreateStockHandler(ERP_DbContext context) => _context = context;

        public async Task<int> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = new Stock
            {
                itemId = request.ItemId,
                quantity = request.Quantity,
                purchasePrice = request.PurchasePrice,
                sellPrice = request.SellPrice,
                purchaseDate = request.PurchaseDate,
                batchNumber = request.BatchNumber,
                barCode = request.BarCode,
                expiryDate = request.ExpiryDate
            };

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync(cancellationToken);
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
        public DateTime ExpiryDate { get; set; }
    }

    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, int>
    {
        private readonly ERP_DbContext _context;
        public UpdateStockHandler(ERP_DbContext context) => _context = context;

        public async Task<int> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return 0;

            stock.quantity = request.Quantity;
            stock.purchasePrice = request.PurchasePrice;
            stock.sellPrice = request.SellPrice;
            stock.purchaseDate = request.PurchaseDate;
            stock.batchNumber = request.BatchNumber;
            stock.barCode = request.BarCode;
            stock.expiryDate = request.ExpiryDate;

            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync(cancellationToken);

            return stock.stockId;
        }
    }
    #endregion

    #region Delete Stock
    public class DeleteStockCommand : IRequest<bool>
    {
        public int StockId { get; set; }
    }

    public class DeleteStockHandler : IRequestHandler<DeleteStockCommand, bool>
    {
        private readonly ERP_DbContext _context;
        public DeleteStockHandler(ERP_DbContext context) => _context = context;

        public async Task<bool> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.stockId == request.StockId && !s.IsDeleted, cancellationToken);
            if (stock == null) return false;

            stock.IsDeleted = true;
            stock.ModifiedOn = DateTime.Now;
            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync(cancellationToken);
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

    public class AdjustStockQuantityHandler : IRequestHandler<AdjustStockQuantityCommand, bool>
    {
        private readonly ERP_DbContext _context;
        public AdjustStockQuantityHandler(ERP_DbContext context) => _context = context;

        public async Task<bool> Handle(AdjustStockQuantityCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return false;

            stock.quantity += request.QuantityAdjustment;

            // Ensure stock is not negative
            if (stock.quantity < 0)
            {
                return false;
            }

            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync(cancellationToken);

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

    public class UpdateStockPricesHandler : IRequestHandler<UpdateStockPricesCommand, bool>
    {
        private readonly ERP_DbContext _context;
        public UpdateStockPricesHandler(ERP_DbContext context) => _context = context;

        public async Task<bool> Handle(UpdateStockPricesCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(request.StockId);
            if (stock == null || stock.IsDeleted) return false;

            stock.purchasePrice = request.NewPurchasePrice;
            stock.sellPrice = request.NewSellPrice;

            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
    #endregion
}
