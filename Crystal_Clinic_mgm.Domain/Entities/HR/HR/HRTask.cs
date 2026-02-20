using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class HRTask : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public int? TemplateLineId { get; set; }
        public OnboardingTaskTemplateLine? TemplateLine { get; set; }

        public string TaskName { get; set; } = string.Empty;
        public HRTaskCategory Category { get; set; }
        public HRTaskStatus Status { get; set; } = HRTaskStatus.Pending;
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public int? AssignedToEmployeeId { get; set; }
        public EmployeeProfile? AssignedToEmployee { get; set; }

        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
}
