using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Command.Create
{
    public class CreatePartnersCommand :  IRequest<JsonResult>
    {
        public string NameInEnglish { get; set; } = string.Empty;
        public string NameInPashto { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
