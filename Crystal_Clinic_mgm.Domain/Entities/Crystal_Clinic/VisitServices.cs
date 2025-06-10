using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class VisitServices
    {
        public int visitServiceId { get; set; }
        public int visitId { get; set; }
        public int serviceId { get; set; }
        public Service? service { get; set; }
        public DateTime startDate { get; set; }
        public int totalSessions { get; set; }
        public int completedSessions { get; set; }
        public decimal pricePerSession { get; set; }
        public decimal totalPrice { get; set; }
        public decimal paidAmount { get; set; }
        public decimal remainAmount { get; set; }
        public DateTime? nextSessionDate { get; set; }
        public PaymentStatus paymentStatus { get; set; } // Pending or Completed

        // Additional optional properties
        public string? sessionStatus { get; set; } // To track each session status
        public decimal? discount { get; set; } // Optional discount or adjustment to the price
    }

}
