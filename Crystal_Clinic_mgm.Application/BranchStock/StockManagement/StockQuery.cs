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
    }

    public class GetStockHandler : IRequestHandler<GetStockQuery, GetStockResponse>
    {
        private readonly ERP_DbContext _context;

        public GetStockHandler(ERP_DbContext context)
        {
            _context = context;
        }

        public async Task<GetStockResponse> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Stocks
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
}
