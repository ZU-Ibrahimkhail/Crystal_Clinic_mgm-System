using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.PaySalary
{
    public class PaySalaryCommand : IRequest<JsonResult>
    {
        public int PayrollId { get; set; }
        public Guid? MainAccountId { get; set; }
        public int EmployeeId { get; set; }
        public int ContractDetailsId { get; set; }
        public int PayTypeId { get; set; }
        public int BranchId { get; set; }
        public double BaseSalary { get; set; }
        public double AdvanceDeduction { get; set; }
        public double NetSalary { get; set; }
        public bool IsPayed { get; set; } 

    }
}
