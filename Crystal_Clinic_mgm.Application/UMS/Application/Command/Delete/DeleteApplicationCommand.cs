using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Command.Delete
{
    public class DeleteApplicationCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
