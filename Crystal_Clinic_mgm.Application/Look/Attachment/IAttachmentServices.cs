using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public interface IAttachmentServices
{
    Task<JsonResult> CreateAttachment(CreateAttachmentDto dto, CancellationToken cancellationToken);
}
