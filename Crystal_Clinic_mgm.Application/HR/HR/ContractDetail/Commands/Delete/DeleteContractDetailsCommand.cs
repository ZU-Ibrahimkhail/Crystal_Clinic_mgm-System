using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Delete
{
    public class DeleteContractDetailsCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get; set; }
    }
}
