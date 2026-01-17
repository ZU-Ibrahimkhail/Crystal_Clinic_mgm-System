using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public class UpdateAttachmentCommand : IRequest<Result>
{
    public UpdateAttachmentDto Dto { get; set; }
}
public class UpdateAttachmentCommandHandler(
    IGenericRepositoryAsync<ERP_DbContext, Attachments> genericRepository,
    ILoggedInUser loggedInUser) : IRequestHandler<UpdateAttachmentCommand, Result>
{
    public async Task<Result> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await genericRepository.GetDetailAsync(request.Dto.Id);

        if (entity != null)
        {
            string filePath = string.Empty;
            string fileExtension = string.Empty;

            if (request.Dto.File != null && !string.IsNullOrWhiteSpace(request.Dto.File.FileName))
            {
                var storage = new FileHandler();
                fileExtension = Path.GetExtension(request.Dto.File.FileName);

                filePath = await storage.CreateAsync(
                    request.Dto.File.OpenReadStream(),
                    fileExtension,
                    "wwwroot",
                    AppConfig.ClinicAttachment
                );
            }

            entity.FilePath = filePath;
            entity.FileExtention = fileExtension;
            entity.AttachmentType = request.Dto.AttachmentType;
            entity.AttachmentDescription = request.Dto.AttachmentDescription;
            entity.ModifiedBy = loggedInUser.Id;
            entity.ModifiedOn = DateTime.UtcNow;
        }
        await genericRepository.UpdateAsync(entity, cancellationToken);

        return Result.Success(entity.Id);
       

    }
}
