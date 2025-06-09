namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Queries.GetList
{
    public class GetAdvancePaymentListModel
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeSurName { get; set; }
        public int ContractDetailsId { get; set; }
        public int? PayTypeId { get; set; }
        public string? PayType { get; set; }
        public int? BranchId { get; set; }
        public string? Branch { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string? CurrencyType { get; set; }
        public DateTime AdvanceDate { get; set; }
        public double AdvanceAmount { get; set; }
        public double RemainingBalance { get; set; }
        public double EachInstallmentAmount { get; set; }
        public Guid? PayedBy { get; set; }
        public string? PayedByUserName { get; set; }
        public string Remarks { get; set; } = string.Empty;

    }
}
