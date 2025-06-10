using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class VisitMedication : AuditableEntity
    {
        public int medicationId { get; set; }
        public int name { get; set; }
        public string dosage { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int visitId { get; set; }
        public int stockId { get; set; }
        public Stock? stock { get; set; }
    }
}
