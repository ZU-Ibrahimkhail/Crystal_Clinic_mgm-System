using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class CreateAttachmentCommand : IRequest<JsonResult>
{
    public CreateAttachmentDto Dto { get; set; }
}


public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, JsonResult>
{
    private readonly IGenericRepositoryAsync<ERP_DbContext, JsonResult> _genericRepository;

    public CreateAttachmentCommandHandler(IGenericRepositoryAsync<ERP_DbContext, JsonResult> genericRepository)
    {
        _genericRepository = genericRepository;
    }
   
    public async Task<JsonResult> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
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

        return new JsonResult(filePath);
    }
}
