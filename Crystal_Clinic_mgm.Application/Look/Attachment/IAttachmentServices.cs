using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;

namespace Crystal_Clinic_Mgm.Application.Look.Attachment;

public interface IAttachmentServices
{
    Task<Result> CreateAttachment(CreateAttachmentDto dto, CancellationToken cancellationToken);
    Task<Result> UpdateAttachment(UpdateAttachmentDto dto, CancellationToken cancellationToken);

}
