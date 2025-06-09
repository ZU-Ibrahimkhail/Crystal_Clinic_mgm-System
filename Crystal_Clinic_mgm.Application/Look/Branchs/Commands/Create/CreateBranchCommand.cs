using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Create
{
    public class CreateBranchCommand : IRequest<JsonResult>
    {
        public string EnglishName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
