using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Update
{
    public class UpdateLanguageCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}