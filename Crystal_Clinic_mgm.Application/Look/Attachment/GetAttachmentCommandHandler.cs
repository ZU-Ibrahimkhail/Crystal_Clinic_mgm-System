using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public class GetAttachmentQuery : IRequest<JsonResult>
{
    public int Id { get; set; }
}

public class GetAttachmentCommandHandler ( IGenericRepositoryAsync<ERP_DbContext, JsonResult> genericRepository, IMessage message ) : IRequestHandler<GetAttachmentQuery, JsonResult>
{
    public async Task<JsonResult> Handle(GetAttachmentQuery request, CancellationToken cancellationToken)
    {
        var attachemnt = await genericRepository.GetDetailAsync(request.Id);
        if (request.Id > 0)
        {
            if (attachemnt == null)
            {
                return message.RecordNotFound("Attachment not found");
            }
            return new JsonResult(attachemnt);
        }
        else
        {
            return message.IdErrorMessage("Invalid Attachment Id");
        }
       
    }
}
