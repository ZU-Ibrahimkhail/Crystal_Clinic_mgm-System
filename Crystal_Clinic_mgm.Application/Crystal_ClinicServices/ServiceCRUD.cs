using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    #region CreateService
    public class CreateServiceCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal sessionRate { get; set; }
        public int BranchId { get; set; }
        public int CurrencyTypeId { get; set; }
    }

    public class CreateServiceHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateServiceCommand, int>
    {
        public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Service
            {
                Name = request.Name,
                Description = request.Description,
                sessionRate = request.sessionRate,
                BranchId = request.BranchId,
                CurrencyTypeId = request.CurrencyTypeId,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now
            };

            context.Services.Add(service);
            await context.SaveChangesAsync(cancellationToken);
            return service.ServiceId;
        }
    }
    #endregion

    #region UpdateService
    public class UpdateServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal sessionRate { get; set; }
        public int BranchId { get; set; }
        public int CurrencyTypeId { get; set; }


    }

    public class UpdateServiceHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateServiceCommand, bool>
    {
        public async Task<bool> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await context.Services.FindAsync(request.ServiceId);
            if (service == null || service.IsDeleted) return false;

            service.Name = request.Name;
            service.Description = request.Description;
            service.sessionRate = request.sessionRate;
            service.BranchId = request.BranchId;
            service.CurrencyTypeId = request.CurrencyTypeId;
            service.ModifiedBy = loggedInUser.Id;
            service.ModifiedOn = DateTime.Now;

            context.Services.Update(service);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    #region DeleteService
    public class DeleteServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }
    }

    public class DeleteServiceHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteServiceCommand, bool>
    {
        public async Task<bool> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await context.Services.FindAsync(request.ServiceId);
            if (service == null || service.IsDeleted) return false;
            service.IsDeleted = true;
            service.ModifiedOn = DateTime.Now;
            service.ModifiedBy = loggedInUser.Id;
            context.Services.Update(service);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    #region Add Image To service
    public class AddImageToServiceCommand : IRequest<int>
    {
        public int ServiceId { get; set; }
        public IFormFile? FormFile { get; set; }
    }
    public class AddImageServiceHandler(ERP_DbContext context) : IRequestHandler<AddImageToServiceCommand, int>
    {
        public async Task<int> Handle(AddImageToServiceCommand request, CancellationToken cancellationToken)
        {

            var service = await context.Services.FindAsync(request.ServiceId);
            if (service == null || service.IsDeleted) return 0;

            var attachment = request.FormFile;
            string FilePath = "";
            if (attachment != null)
            {
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    await _sotrage.RemoveFile("wwwroot", service.ImagePath);
                    string ext = Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.Reception_RequestAttachment);
                }
            }
            service.ImagePath = FilePath;
            context.Services.Update(service);
            await context.SaveChangesAsync(cancellationToken);

            return service.ServiceId;
        }
    }
    #endregion

    #region GetServiceById
    public class GetServiceByIdQuery : IRequest<Service>
    {
        public int ServiceId { get; set; }
    }

    public class GetServiceByIdHandler(ERP_DbContext context) : IRequestHandler<GetServiceByIdQuery, Service>
    {
        public async Task<Service> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Services.FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId && !s.IsDeleted, cancellationToken) ?? new Service();
        }
    }
    #endregion

    #region ListAllServices

    public class ListAllServicesQuery : IRequest<List<ServiceDto>>
    {
        public string? Search { get; set; }
        public int? LastServiceId { get; set; } // For cursor pagination
        public int PageSize { get; set; } = 20;
    }

    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal sessionRate { get; set; }
        public string? ImagePath { get; set; }
        public int BranchId { get; set; }
        public int CurrencyTypeId { get; set; }
        public string CurrencyTypeName { get; set; } = string.Empty;
    }

    public class ListAllServicesHandler(ERP_DbContext context,IHttpContextAccessor httpContextAccessor) : IRequestHandler<ListAllServicesQuery, List<ServiceDto>>
    {
        public async Task<List<ServiceDto>> Handle(ListAllServicesQuery request, CancellationToken cancellationToken)
        {
            var query = context.Services.Where(x => !x.IsDeleted).AsQueryable();

            Localization localize = new(httpContextAccessor);
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.Name.Contains(request.Search));
            }

            if (request.LastServiceId.HasValue)
            {
                query = query.Where(s => s.ServiceId > request.LastServiceId.Value);
            }

            query = query.Include(x=>x.CurrencyType).OrderBy(s => s.ServiceId).Take(request.PageSize);

            return await query.Select(s => new ServiceDto
            {
                ServiceId = s.ServiceId,
                Name = s.Name,
                Description = s.Description,
                sessionRate = s.sessionRate,
                CurrencyTypeId = s.CurrencyTypeId,
                CurrencyTypeName = localize.GetName(s.CurrencyType),
                ImagePath = s.ImagePath
            }).ToListAsync(cancellationToken);
        }
    }
    #endregion
}
