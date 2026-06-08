using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class VisitInstrument : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public Visit Visit { get; set; } = null!;
        public decimal DoctorFee { get; set; }
        public int? VisitKitsId { get; set; }
        public VisitKits? VisitKits { get; set; }
        public int? ServiceSessionsId { get; set; }
        public ServiceSessions? ServiceSessions { get; set; }
        public int? VisitMedicationId { get; set; }
        public VisitMedication? VisitMedication { get; set; }
        public bool IsFreeForPatient { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
