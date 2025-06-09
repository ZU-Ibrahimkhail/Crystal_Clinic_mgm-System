using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Delete
{
    public class DeletePayTypeCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get;  set; }
    }
}
