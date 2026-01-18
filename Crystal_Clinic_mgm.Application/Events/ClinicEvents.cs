using MediatR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic.Events
{
    public class MedicationDispensedEvent : INotification
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public IEnumerable<DispensedMedication> Medications { get; set; } = new List<DispensedMedication>();
        public DateTime DispensedAt { get; set; } = DateTime.UtcNow;
        public string ReferenceId { get; set; } = string.Empty;
    }

    public class DispensedMedication
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string LotNumber { get; set; } = string.Empty;
    }
}