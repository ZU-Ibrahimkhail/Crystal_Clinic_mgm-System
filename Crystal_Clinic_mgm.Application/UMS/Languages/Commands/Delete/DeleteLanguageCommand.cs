
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Delete
{
    public class DeleteLanguageCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}