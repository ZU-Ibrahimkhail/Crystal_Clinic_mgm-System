using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Command.Update
{
    public class UpdatePartnersCommand :  IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string NameInEnglish { get; set; } = string.Empty;
        public string NameInPashto { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
