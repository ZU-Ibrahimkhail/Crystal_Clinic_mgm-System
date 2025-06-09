using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Create
{
    public class CreateAdvancePaymentCommand : IRequest<JsonResult>
    {
        public Guid MainAccountId { get; set; }
        public int EmployeeId { get; set; }
        public int PayTypeId { get; set; }
        public DateTime AdvanceDate { get; set; }
        public double AdvanceAmount { get; set; }
        public double EachInstallmentAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
