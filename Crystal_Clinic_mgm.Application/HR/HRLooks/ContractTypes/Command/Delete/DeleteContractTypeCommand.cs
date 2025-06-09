using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Delete
{
    public class DeleteContractTypeCommand : IRequest<JsonResult>
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
