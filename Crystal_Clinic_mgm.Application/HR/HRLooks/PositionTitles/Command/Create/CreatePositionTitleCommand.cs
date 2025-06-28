using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Create
{
    public class CreatePositionTitleCommand : IRequest<JsonResult>
    {
        public string EnglishName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public bool IsActive { get; set; } = false;
        public string JobDescription { get; set; } = string.Empty;

    }
}
