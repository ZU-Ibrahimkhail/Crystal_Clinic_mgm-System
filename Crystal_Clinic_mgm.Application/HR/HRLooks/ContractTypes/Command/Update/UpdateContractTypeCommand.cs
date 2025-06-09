using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Update
{
    public class UpdateContractTypeCommand : IRequest<JsonResult>
    {
        public int Id { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
