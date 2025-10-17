using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class CallList : AuditableEntity
    {
        public int Id { get; set; }
        public CallingReason CallingReason { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ToBeCalledDate { get; set; }
        public DateTime? ActualCalledDate { get; set; }
        public CallResponseType CallResponse { get; set; }
        public string ResponseReasult { get; set; } = string.Empty;
        public int? AssignedEmployeeId { get; set; }
        public EmployeeProfile? AssignedEmployee { get; set; }

    }
}
