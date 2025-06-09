using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class Return
    {
        public int ReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal DamageFee { get; set; }
        public decimal DepositRefund { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public Orders? Order { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
