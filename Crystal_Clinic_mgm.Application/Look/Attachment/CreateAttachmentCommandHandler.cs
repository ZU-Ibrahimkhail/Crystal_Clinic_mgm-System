using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;

public class CreateAttachmentCommand : IRequest<Result>
{
    public CreateAttachmentDto Dto { get; set; }
}


public class CreateAttachmentCommandHandler(
    IGenericRepositoryAsync<ERP_DbContext, Attachments> genericRepository,
    ILoggedInUser _loggedInUser)
    : IRequestHandler<CreateAttachmentCommand, Result>
{
   
   
    public async Task<Result> Handle(
        CreateAttachmentCommand request,
        CancellationToken cancellationToken)
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

        var attachment = new Attachments
        {
            FilePath = filePath,
            FileExtention = fileExtension,
            AttachmentType = request.Dto.AttachmentType,
            AttachmentDescription = request.Dto.AttachmentDescription,
            CreatedBy = _loggedInUser.Id,
            CreatedOn = DateTime.UtcNow
        };

        await genericRepository.AddAsync(attachment, cancellationToken);

        return Result.Success(attachment.Id);
    }
}
