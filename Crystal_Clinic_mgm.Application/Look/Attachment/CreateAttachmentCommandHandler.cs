using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class CreateAttachmentCommand : IRequest<JsonResult>
{
    public AttachmentType AttachmentType { get; set; }
    public IFormFile File { get; set; } = null!;
}


public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, JsonResult>
{
    private static class AttachmentPathResolver
    {
        public static string ResolveFolder(AttachmentType type)
        {
            var year = DateTime.Now.Year.ToString();
            return type switch
            {
                AttachmentType.PatientDocument => "PatientDocuments",
                AttachmentType.SalesInvoice => "SalesInvoice",
                AttachmentType.PurchaseOrder => "PurchaseOrder",
                AttachmentType.ShareHolder => "ShareHolder",
                AttachmentType.AccountReceivable => "AccountReceivable",
                AttachmentType.AccountPayable => "AccountPayable",
                AttachmentType.VendorBill => "VendorBill",
                AttachmentType.Item => "Item",
                AttachmentType.JournalEntry => "JournalEntry",
                _ => "Others"
            };
        }
    }


    public async Task<JsonResult> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || string.IsNullOrWhiteSpace(request.File.FileName))
            return new JsonResult(string.Empty);

        var fileExtension = Path.GetExtension(request.File.FileName);
        var subFolder = AttachmentPathResolver.ResolveFolder(request.AttachmentType);

        var storage = new FileHandler();

        var savedPath = await storage.CreateAsync(
            request.File.OpenReadStream(),
            fileExtension,
            "wwwroot",
            Path.Combine(AppConfig.ClinicAttachment, subFolder)
        );

        var urlPath = "/" + savedPath.Replace("\\", "/").TrimStart('/');

        return new JsonResult(new { attachment = urlPath });
    }
}
