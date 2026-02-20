namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class OnboardingTaskTemplateLine : AuditableEntity
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }
        public OnboardingTaskTemplate? Template { get; set; }

        public string TaskName { get; set; } = string.Empty;
        public string? TaskDescription { get; set; }
        public int TaskOrder { get; set; }
        public bool IsRequired { get; set; } = true;
        public int? AssignedToDepartmentId { get; set; }
        public string? AssignedToRole { get; set; }
    }
}
