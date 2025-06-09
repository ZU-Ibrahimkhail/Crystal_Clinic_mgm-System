using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Delete
{
    public class DeleteCurrencyTypeCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get;  set; }
    }
}
