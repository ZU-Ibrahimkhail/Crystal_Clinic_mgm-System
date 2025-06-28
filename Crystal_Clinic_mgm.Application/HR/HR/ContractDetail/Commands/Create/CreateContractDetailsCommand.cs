using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Create
{
    public class CreateContractDetailsCommand : IRequest<JsonResult>
    {
        public int EmployeeProfileId { get; set; }
        public int ContractTypeId { get; set; }
        public int CurrencyTypeId { get; set; }
        public int PositionTitleId { get; set; }
        public int BranchId { get; set; }
        public double SalaryAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } = null;
        public bool IsActive { get; set; } = false;
        public string Remarks { get; set; } = string.Empty;
        public IFormFile? Attachment { get; set; }

    }
}
