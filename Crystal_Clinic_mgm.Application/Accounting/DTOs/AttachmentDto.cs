using Crystal_Clinic_Mgm.Domain;
using Microsoft.AspNetCore.Http;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs;

public class CreateAttachmentDto
{
    public AttachmentType AttachmentType { get; set; }
    public IFormFile File { get; set; } = null!;
    public string AttachmentDescription { get; set; } = string.Empty;
}

public class UpdateAttachmentDto
{
    public int Id { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public IFormFile? File { get; set; }
    public string AttachmentDescription { get; set; } = string.Empty;
}
