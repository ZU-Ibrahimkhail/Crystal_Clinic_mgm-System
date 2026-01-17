using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public class AttachemntServices(IMediator mediator) : IAttachmentServices
{
    public async Task<Result> CreateAttachment(CreateAttachmentDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateAttachmentCommand { Dto = dto };
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<Result> UpdateAttachment(UpdateAttachmentDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateAttachmentCommand { Dto = dto };
        return await mediator.Send(command, cancellationToken);
    }
}
