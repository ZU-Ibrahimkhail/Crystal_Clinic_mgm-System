using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Delete
{
    public class DeleteAdvancePaymentCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get; set; }
    }
}
