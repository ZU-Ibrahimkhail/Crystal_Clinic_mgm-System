using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Update
{
    public class UpdateAdvancePaymentCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public int PayTypeId { get; set; }
        public DateTime AdvanceDate { get; set; }
        public double AdvanceAmount { get; set; }
        public double EachInstallmentAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
