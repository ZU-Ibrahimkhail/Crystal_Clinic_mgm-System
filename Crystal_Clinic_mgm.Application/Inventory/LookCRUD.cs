// ItemCategory + Item + ItemUnit CRUD Commands
using System.Dynamic;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Inventory.Commands
{
    //---------------------------------
    #region Create ItemCategory
    public class CreateItemCategoryCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateItemCategoryHandler : IRequestHandler<CreateItemCategoryCommand, int>
    {
        private readonly ERP_DbContext _context;
        public CreateItemCategoryHandler(ERP_DbContext context) => _context = context;

        public async Task<int> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ItemCategory { Name = request.Name, Description = request.Description };
            _context.ItemCategories.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.categoryId;
        }
    }
    #endregion

    #region Update ItemCategory
    public class UpdateItemCategoryCommand : CreateItemCategoryCommand, IRequest<int>
    {
        public int ItemCategoryId { get; set; }
    }

    public class UpdateItemCategoryHandler(ERP_DbContext context) : IRequestHandler<UpdateItemCategoryCommand, int>
    {
        public async Task<int> Handle(UpdateItemCategoryCommand request, CancellationToken cancellationToken)
        {

            var entity = await context.ItemCategories.FindAsync(request.ItemCategoryId);
            if (entity == null) return 0;
            entity.Name = request.Name;
            entity.Description = request.Description;
            context.ItemCategories.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity.categoryId;
        }
    }
    #endregion

    #region Delete ItemCategory
    public class DeleteItemCategoryCommand : IRequest<bool>
    {
        public int CategoryId { get; set; }
    }

    public class DeleteItemCategoryHandler(ERP_DbContext context) : IRequestHandler<DeleteItemCategoryCommand, bool>
    {
        public async Task<bool> Handle(DeleteItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.ItemCategories.FindAsync(request.CategoryId);
            if (category == null) return false;
            context.ItemCategories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    //----------------------------------

    #region Create Item with ItemUnits
    public class CreateItemWithUnitsCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSellable { get; set; }
        public bool IsRentable { get; set; }
        public string BaseUnit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal RealTimeAvailableStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public decimal CleaningStateQuantity { get; set; } = 0;

        public List<ItemUnitDto> Units { get; set; } = new();
    }

    public class ItemUnitDto
    {
        public string UnitName { get; set; } = string.Empty;
        public decimal ConversionFactor { get; set; }
        public int? DailyRentalPrice { get; set; }
        public int SellingPrice { get; set; }
    }

    public class CreateItemWithUnitsHandler(ERP_DbContext context) : IRequestHandler<CreateItemWithUnitsCommand, int>
    {
        public async Task<int> Handle(CreateItemWithUnitsCommand request, CancellationToken cancellationToken)
        {

            var item = new Item
            {
                Name = request.Name,
                BranchId = request.BranchId,
                Description = request.Description,
                IsSellable = request.IsSellable,
                IsRentable = request.IsRentable,
                BaseUnit = request.BaseUnit,
                CurrentStock = request.CurrentStock,
                RealTimeAvailableStock = request.RealTimeAvailableStock,
                CleaningStateQuantity = request.CleaningStateQuantity,
                ReorderLevel = request.ReorderLevel,
                CategoryId = request.CategoryId
            };

            context.Items.Add(item);
            await context.SaveChangesAsync(cancellationToken);

            foreach (var unit in request.Units)
            {
                var itemUnit = new ItemUnit
                {
                    ItemId = item.ItemId,
                    UnitName = unit.UnitName,
                    ConversionFactor = unit.ConversionFactor,
                    DailyRentalPrice = unit.DailyRentalPrice,
                    SellingPrice = unit.SellingPrice
                };
                context.ItemUnits.Add(itemUnit);
            }

            await context.SaveChangesAsync(cancellationToken);
            return item.ItemId;
        }
    }
    #endregion

    #region Update Item
    public class UpdateItemCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSellable { get; set; }
        public bool IsRentable { get; set; }
        public string BaseUnit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal CleaningStateQuantity { get; set; } = 0;
        public decimal ReorderLevel { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public int ItemId { get; set; }
    }
    public class UpdateItemWithUnitsHandler(ERP_DbContext context) : IRequestHandler<UpdateItemCommand, int>
    {
        public async Task<int> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {

            var item = await context.Items.FindAsync(request.ItemId);
            if (item == null) return 0;

            var Defrience = item.CurrentStock - request.CurrentStock;

            item.Name = request.Name;
            item.BranchId = request.BranchId;
            item.Description = request.Description;
            item.RealTimeAvailableStock += (item.CleaningStateQuantity - request.CleaningStateQuantity);
            item.CleaningStateQuantity = request.CleaningStateQuantity;
            item.IsSellable = request.IsSellable;
            item.IsRentable = request.IsRentable;
            item.BaseUnit = request.BaseUnit;
            item.CurrentStock = request.CurrentStock;
            item.ReorderLevel = request.ReorderLevel;
            item.RealTimeAvailableStock -= Defrience;
            item.CategoryId = request.CategoryId;
            context.Items.Update(item);
            await context.SaveChangesAsync(cancellationToken);

            return item.ItemId;
        }
    }
    #endregion

    #region Add Image To Item
    public class AddImageToItemCommand : IRequest<int>
    {
        public int ItemId { get; set; }
        public IFormFile? FormFile { get; set; }
    }
    public class AddImageToItemWithUnitsHandler(ERP_DbContext context) : IRequestHandler<AddImageToItemCommand, int>
    {
        public async Task<int> Handle(AddImageToItemCommand request, CancellationToken cancellationToken)
        {

            var item = await context.Items.FindAsync(request.ItemId);
            if (item == null) return 0;

            var attachment = request.FormFile;
            string FilePath = "";
            if (attachment != null)
            {
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    await _sotrage.RemoveFile("wwwroot",item.ImagePath);
                    string ext = Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.Reception_RequestAttachment);
                }
            }
            item.ImagePath = FilePath;
            context.Items.Update(item);
            await context.SaveChangesAsync(cancellationToken);

            return item.ItemId;
        }
    }
    #endregion

    #region Add Unit to Item
    public class AddItemUnitCommand : IRequest<int>
    {
        public int ItemId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public decimal ConversionFactor { get; set; }
        public int? DailyRentalPrice { get; set; }
        public int SellingPrice { get; set; }
    }
    public class AddItemUnitHandler(ERP_DbContext context) : IRequestHandler<AddItemUnitCommand, int>
    {
        public async Task<int> Handle(AddItemUnitCommand request, CancellationToken cancellationToken)
        {


            var itemUnit = new ItemUnit
            {
                ItemId = request.ItemId,
                UnitName = request.UnitName,
                ConversionFactor = request.ConversionFactor,
                DailyRentalPrice = request.DailyRentalPrice,
                SellingPrice = request.SellingPrice
            };
            context.ItemUnits.Add(itemUnit);

            await context.SaveChangesAsync(cancellationToken);
            return itemUnit.unitId;
        }
    }
    #endregion

    #region Update Unit Item
    public class UpdateItemUnitCommand : IRequest<int>
    {
        public int unitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public decimal ConversionFactor { get; set; }
        public int? DailyRentalPrice { get; set; }
        public int SellingPrice { get; set; }
    }
    public class UpdateItemUnitHandler(ERP_DbContext context) : IRequestHandler<UpdateItemUnitCommand, int>
    {
        public async Task<int> Handle(UpdateItemUnitCommand request, CancellationToken cancellationToken)
        {


            var itemUnit = await context.ItemUnits.FindAsync(request.unitId);
            if (itemUnit == null) return 0;
            
            itemUnit.UnitName = request.UnitName;
            itemUnit.ConversionFactor = request.ConversionFactor;
            itemUnit.DailyRentalPrice = request.DailyRentalPrice;
            itemUnit.SellingPrice = request.SellingPrice;
            context.ItemUnits.Update(itemUnit);

            await context.SaveChangesAsync(cancellationToken);
            return itemUnit.unitId;
        }
    }
    #endregion

    #region Delete Unit from Item

    public class DeleteItemUnitCommand : IRequest<int>
    {
        public int UnitId { get; set; }
    }
    public class DeleteItemUnitHandler(ERP_DbContext context) : IRequestHandler<DeleteItemUnitCommand, int>
    {
        public async Task<int> Handle(DeleteItemUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await context.ItemUnits.FindAsync(request.UnitId);
            if (unit == null) return 0;
            context.ItemUnits.Remove(unit);
            await context.SaveChangesAsync(cancellationToken);
            return unit.unitId;
        }
    }
    #endregion

    #region Delete Item
    public class DeleteItemCommand : IRequest<bool>
    {
        public int ItemId { get; set; }
    }

    public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly ERP_DbContext _context;
        public DeleteItemHandler(ERP_DbContext context) => _context = context;

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.Include(i => i.Units)
                .FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            if (item == null) return false;

            _context.ItemUnits.RemoveRange(item.Units);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    //--------------------------------

}
