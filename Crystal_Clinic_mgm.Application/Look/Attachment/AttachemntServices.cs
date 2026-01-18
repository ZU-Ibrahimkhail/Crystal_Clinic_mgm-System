 using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public class AttachemntServices : IAttachmentServices
{
    private readonly IMediator _mediator;
    public AttachemntServices(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<JsonResult> CreateAttachment(CreateAttachmentDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateAttachmentCommand { Dto = dto };
        return await _mediator.Send(command, cancellationToken);
    }
}