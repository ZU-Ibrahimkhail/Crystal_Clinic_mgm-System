using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock._Stock
{
    #region Get Stock List with Pagination & Search
    public class GetStockQuery : IRequest<GetStockResponse>
    {
        public string? SearchText { get; set; }
        [Required]
        public int BranchId { get; set; } = 1; // Default to branch 1
        public int PageSize { get; set; } = 20; // Default page size
        public int? LastItemId { get; set; } // For pagination based on last item ID
        public int? CategoryId { get; set; } // For filtering based on item category ID
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
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
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
                .Include(s => s.Item) // Include related item data
                .Include(s => s.Supplier) // Include related item data
                .Where(s => !s.IsDeleted && s.BranchId == request.BranchId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(s => 
                s.Item!.Name.Contains(request.SearchText) ||
                s.BarCode.Contains(request.SearchText) ||
                s.Supplier!.Name.Contains(request.SearchText)
                );
            }

            if (request.LastItemId.HasValue)
            {
                query = query.Where(s => s.StockId > request.LastItemId.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(s => s.Item != null && s.Item.CategoryId == request.CategoryId.Value);
            }

            var stockItems = await query
                .OrderBy(s => s.StockId)
                .Take(request.PageSize)
                .Select(s => new StockDto
                {
                    StockId = s.StockId,
                    ItemId = s.ItemId ?? 0,
                    ItemName = s.Item!.Name,
                    SupplierId = s.SupplierId,
                    SupplierName = s.Supplier!.Name,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    PurchasePrice = s.PurchasePrice,
                    SellPrice = s.SellPrice,
                    PurchaseDate = s.PurchaseDate,
                    ExpiryDate = s.ExpiryDate,
                    BarCode = s.BarCode,
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
                .Include(s => s.Item) // Include related item data
                .Include(s => s.Supplier) // Include related item data
                .Where(s => !s.IsDeleted && s.BranchId == request.BranchId && s.Quantity > 0 && s.BarCode.Equals(request.Barcode))
                .AsQueryable();
          
            return await query
                .OrderBy(s => s.ExpiryDate) 
                .ThenBy(s => s.StockId) 
                .Take(1)
                .Select(s => new StockDto
                {
                    StockId = s.StockId,
                    ItemId = s.ItemId ?? 0,
                    ItemName = s.Item!.Name,
                    SupplierId = s.SupplierId,
                    SupplierName = s.Supplier!.Name,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    PurchasePrice = s.PurchasePrice,
                    SellPrice = s.SellPrice,
                    PurchaseDate = s.PurchaseDate,
                    ExpiryDate = s.ExpiryDate,
                    BarCode = s.BarCode
                }).FirstOrDefaultAsync(cancellationToken);

        }
    }



}

