// ItemCategory and Item Queries (List, Search, Pagination)
using System.Text.Json.Serialization;
using Crystal_Clinic_Mgm.Application.Inventory.Commands;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Inventory.Queries
{
    #region Get Category List
    public class GetItemCategoriesQuery : IRequest<List<ItemCategory>> { }

    public class GetItemCategoriesHandler : IRequestHandler<GetItemCategoriesQuery, List<ItemCategory>>
    {
        private readonly ERP_DbContext _context;
        public GetItemCategoriesHandler(ERP_DbContext context) => _context = context;

        public async Task<List<ItemCategory>> Handle(GetItemCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.ItemCategories.OrderBy(x => x.Name).ToListAsync(cancellationToken);
        }
    }
    #endregion

    #region Get Item List with Pagination & Search
    public class GetItemsQuery : IRequest<GetItemsResponse>
    {
        [JsonIgnore]
        public bool isRental { get; set; } = true;
        public string? SearchText { get; set; }
        public int BranchId { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? LastItemId { get; set; }
    }

    public class GetItemsResponse
    {
        public List<ItemDto> Items { get; set; } = new();
        public int? LastItemId { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string BaseUnit { get; set; } = string.Empty;
        public decimal RealTimeAvailableStock { get; set; }
        public decimal CleaningStateQuantity { get; set; } = 0;
        public decimal ReorderLevel { get; set; }
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
        public bool IsSellable { get; set; }
        public bool IsRentable { get; set; }
        public decimal CurrentStock { get; set; }
        public List<ItemUnitsDto> Units { get; set; } = [];
    }

    public class GetItemsHandler(ERP_DbContext context) : IRequestHandler<GetItemsQuery, GetItemsResponse>
    {
        public async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .Include(i => i.Units).Where(x=>x.BranchId == request.BranchId && x.IsRentable == request.isRental)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(i => i.Name.Contains(request.SearchText));
            }

            if (request.LastItemId.HasValue)
            {
                query = query.Where(i => i.ItemId > request.LastItemId.Value);
            }

            var items = await query
                .OrderBy(i => i.ItemId)
                .Take(request.PageSize)
                .Select(i => new ItemDto
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Description = i.Description,
                    ImagePath = i.ImagePath,
                    BaseUnit = i.BaseUnit,
                    RealTimeAvailableStock = i.RealTimeAvailableStock,
                    CleaningStateQuantity = i.CleaningStateQuantity,
                    ReorderLevel = i.ReorderLevel,
                    BranchId = i.BranchId,
                    CategoryId = i.CategoryId,
                    IsSellable = i.IsSellable,
                    IsRentable = i.IsRentable,
                    CurrentStock = i.CurrentStock,
                    Units = i.Units.Select(u => new ItemUnitsDto
                    {
                        unitId = u.unitId,
                        UnitName = u.UnitName,
                        ConversionFactor = u.ConversionFactor,
                        DailyRentalPrice = u.DailyRentalPrice,
                        SellingPrice = u.SellingPrice
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new GetItemsResponse
            {
                Items = items,
                LastItemId = items.LastOrDefault()?.ItemId
            };
        }


    }
    public class ItemUnitsDto : ItemUnitDto
    {
        public int unitId { get; set; }
    }
    #endregion
}
