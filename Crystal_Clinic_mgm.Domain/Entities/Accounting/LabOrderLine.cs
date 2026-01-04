using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class LabOrderLine : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public Visit? Visit { get; set; }
        public int TemplateId { get; set; }
        public LabTestTemplate? Template { get; set; }
        public decimal? ActualValue { get; set; }
        public string? ActualResult { get; set; }
        public bool IsAbnormal { get; set; } = false;
        public DateTime OrderDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
