// ItemCategory + Item CRUD Commands
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory
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
            if (entity == null || entity.IsDeleted) return 0;
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
            if (category == null || category.IsDeleted) return false;
            category.IsDeleted = true;
            category.ModifiedOn = DateTime.Now;
            context.ItemCategories.Update(category);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    //----------------------------------

    #region Create Item
    public class CreateItemCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BaseUnit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal? UseableStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }

    }


    public class CreateItemHandler(ERP_DbContext context) : IRequestHandler<CreateItemCommand, int>
    {
        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {

            var item = new Item
            {
                Name = request.Name,
                BranchId = request.BranchId,
                Description = request.Description,
                BaseUnit = request.BaseUnit,
                CurrentStock = request.CurrentStock,
                UseableStock = request.UseableStock,
                ReorderLevel = request.ReorderLevel,
                CategoryId = request.CategoryId
            };

            context.Items.Add(item);
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
        public string BaseUnit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal? UseableStock { get; set; }
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
            if (item == null || item.IsDeleted) return 0;

            item.Name = request.Name;
            item.BranchId = request.BranchId;
            item.Description = request.Description;
            item.UseableStock = request.UseableStock;
            item.BaseUnit = request.BaseUnit;
            item.CurrentStock = request.CurrentStock;
            item.ReorderLevel = request.ReorderLevel;
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
            if (item == null || item.IsDeleted) return 0;

            var attachment = request.FormFile;
            string FilePath = "";
            if (attachment != null)
            {
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    await _sotrage.RemoveFile("wwwroot", item.ImagePath);
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
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.ItemId == request.ItemId && !i.IsDeleted, cancellationToken);
            if (item == null) return false;

            item.IsDeleted = true;
            item.ModifiedOn = DateTime.Now;
            _context.Items.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    //--------------------------------

}
