using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class Visit : AuditableEntity
    {
        public int visitId { get; set; }
        public int BranchId { get; set; } = 1;
        public int? BranchDetailsId { get; set; }
        public BranchDetails? BranchDetails { get; set; }
        public int patientId { get; set; }
        public Patient? Patient { get; set; } // Navigation property
        public int? doctorId { get; set; }
        public Doctor? Doctor { get; set; } // Navigation property
        public DateTime visitDate { get; set; }
        public VisitStatus status { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal totalAmount { get; set; }
        public decimal paidAmount { get; set; }
        public decimal remainingAmount { get; set; }
        public ICollection<VisitMedication> Medications { get; set; } = [];
        public ICollection<VisitServices> Services { get; set; } = [];
        public ICollection<VisitPayment> Payments { get; set; } = [];
        public bool IsCompleted => status == VisitStatus.COMPLETED;
    }

    public enum VisitStatus
    {
        SCHEDULED,
        PENDING,
        IN_PROCESS,
        COMPLETED
    }
}
