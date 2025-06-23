namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class Visit : AuditableEntity
    {
        public int visitId { get; set; }

        // Foreign key to the Patient table
        public int patientId { get; set; }
        public Patient? Patient { get; set; } // Navigation property

        // Foreign key to the Doctor table (nullable, as not all visits may have a doctor assigned at the time of creation)
        public int? doctorId { get; set; }
        public Doctor? Doctor { get; set; } // Navigation property

        public DateTime visitDate { get; set; }

        // The status of the visit
        public VisitStatus status { get; set; }

        // Total amount of the visit (including services, medications, etc.)
        public decimal totalAmount { get; set; }

        // Amount paid by the patient
        public decimal paidAmount { get; set; }

        // Remaining amount to be paid
        public decimal remainingAmount { get; set; }
        // Collection of medications for the visit (no need for a separate Prescription table)
        public ICollection<VisitMedication> Medications { get; set; } = [];

        // Collection of services (like PRP, laser treatment, etc.)
        public ICollection<VisitServices> Services { get; set; } = [];
        public ICollection<VisitPayment> Payments { get; set; } = [];

        // The ID of the doctor assigned to the visit, which is nullable for cases where no doctor is selected
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
