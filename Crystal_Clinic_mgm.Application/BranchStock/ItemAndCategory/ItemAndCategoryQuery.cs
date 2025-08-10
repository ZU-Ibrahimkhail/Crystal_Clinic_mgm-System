// ItemCategory and Item Queries (List, Search, Pagination)
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory
{
    #region Get Category List
    public class GetItemCategoriesQuery : IRequest<List<ItemCategory>> { }

    public class GetItemCategoriesHandler : IRequestHandler<GetItemCategoriesQuery, List<ItemCategory>>
    {
        private readonly ERP_DbContext _context;
        public GetItemCategoriesHandler(ERP_DbContext context) => _context = context;

        public async Task<List<ItemCategory>> Handle(GetItemCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.ItemCategories.Where(x=>!x.IsDeleted).OrderBy(x => x.Name).ToListAsync(cancellationToken);
        }
    }
    #endregion

    #region Get Item List with Pagination & Search
    public class GetItemsQuery : IRequest<GetItemsResponse>
    {
        public string? SearchText { get; set; }
        public int BranchId { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? LastItemId { get; set; }
        public int? CategoryId { get; set; }
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
        public decimal CurrentStock { get; set; }
        public decimal? UseableStock { get; set; } = 0;
        public decimal ReorderLevel { get; set; }
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class GetItemsHandler(ERP_DbContext context) : IRequestHandler<GetItemsQuery, GetItemsResponse>
    {
        public async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items.Where(x => !x.IsDeleted && x.BranchId == request.BranchId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(i => i.Name.Contains(request.SearchText));
            }

            if (request.LastItemId.HasValue)
            {
                query = query.Where(i => i.ItemId > request.LastItemId.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId > request.CategoryId.Value);
            }

            var items = await query
                .OrderBy(i => i.ItemId)
                .Take(request.PageSize).Include(i => i.Category)
                .Select(i => new ItemDto
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Description = i.Description,
                    ImagePath = i.ImagePath,
                    BaseUnit = i.BaseUnit,
                    UseableStock = i.UseableStock,
                    ReorderLevel = i.ReorderLevel,
                    BranchId = i.BranchId,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category!.Name ?? "",
                    CurrentStock = i.CurrentStock,
                })
                .ToListAsync(cancellationToken);

            return new GetItemsResponse
            {
                Items = items,
                LastItemId = items.LastOrDefault()?.ItemId
            };
        }


    }

    #endregion
}
