using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Stock
{
    #region Get Stock List with Pagination & Search
    public class GetStockQuery : IRequest<GetStockResponse>
    {
        public string? SearchText { get; set; }
        [Required]
        public int BranchId { get; set; } = 1; // Default to branch 1
        public int PageSize { get; set; } = 20; // Default page size
        public int? LastItemId { get; set; } // For pagination based on last item ID
    }

    public class GetStockResponse
    {
        public List<StockDto> StockItems { get; set; } = new();
        public int? LastItemId { get; set; }
    }

    public class StockDto
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? BarCode { get; set; }
    }

    public class GetStockHandler(ERP_DbContext context) : IRequestHandler<GetStockQuery, GetStockResponse>
    {
        public async Task<GetStockResponse> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var query = context.Stocks
                .Include(s => s.item) // Include related item data
                .Where(s => !s.IsDeleted && s.BranchId == request.BranchId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(s => s.item.Name.Contains(request.SearchText));
            }

            if (request.LastItemId.HasValue)
            {
                query = query.Where(s => s.stockId > request.LastItemId.Value);
            }

            var stockItems = await query
                .OrderBy(s => s.stockId)
                .Take(request.PageSize)
                .Select(s => new StockDto
                {
                    StockId = s.stockId,
                    ItemId = s.itemId,
                    ItemName = s.item!.Name,
                    BatchNumber = s.batchNumber,
                    Quantity = s.quantity,
                    PurchasePrice = s.purchasePrice,
                    SellPrice = s.sellPrice,
                    PurchaseDate = s.purchaseDate,
                    ExpiryDate = s.expiryDate
                })
                .ToListAsync(cancellationToken);

            return new GetStockResponse
            {
                StockItems = stockItems,
                LastItemId = stockItems.LastOrDefault()?.StockId
            };
        }
    }



    #endregion

    public class GetStockByBarcodeQuery : IRequest<StockDto?>
    {
        [Required]
        public string Barcode { get; set; }= string.Empty;
        [Required]
        public int BranchId { get; set; } = 1; 
    }



    public class GetStockByBarcodeHandler(ERP_DbContext context) : IRequestHandler<GetStockByBarcodeQuery, StockDto?>
    {
        public async Task<StockDto?> Handle(GetStockByBarcodeQuery request, CancellationToken cancellationToken)
        {
            var query = context.Stocks
                .Include(s => s.item) // Include related item data
                .Where(s => !s.IsDeleted && s.BranchId == request.BranchId && s.quantity > 0 && s.barCode.Equals(request.Barcode))
                .AsQueryable();
          
            return await query
                .OrderBy(s => s.expiryDate) 
                .ThenBy(s => s.stockId) 
                .Take(1)
                .Select(s => new StockDto
                {
                    StockId = s.stockId,
                    ItemId = s.itemId,
                    ItemName = s.item!.Name,
                    BatchNumber = s.batchNumber,
                    Quantity = s.quantity,
                    PurchasePrice = s.purchasePrice,
                    SellPrice = s.sellPrice,
                    PurchaseDate = s.purchaseDate,
                    ExpiryDate = s.expiryDate,
                    BarCode = s.barCode
                }).FirstOrDefaultAsync(cancellationToken);

        }
    }



}

