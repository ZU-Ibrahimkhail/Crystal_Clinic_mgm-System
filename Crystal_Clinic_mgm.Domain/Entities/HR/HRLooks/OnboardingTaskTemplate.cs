namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class OnboardingTaskTemplate : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public HRTaskCategory Category { get; set; } = HRTaskCategory.Onboarding;
        public bool IsActive { get; set; } = true;

        public ICollection<OnboardingTaskTemplateLine> TemplateLines { get; set; } = new List<OnboardingTaskTemplateLine>();
    }
}
