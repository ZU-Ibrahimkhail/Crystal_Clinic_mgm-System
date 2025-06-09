using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Delete
{
    public class DeletePositionTitleCommand : IRequest<JsonResult>
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
