namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList
{
    public class GetPayrollTrackingListModel
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeSurName { get; set; }
        public int ContractDetailsId { get; set; }
        public int PositionTitleId { get; set; }
        public string? PositionTitle { get; set; }
        public int? PayTypeId { get; set; }
        public string? PayType { get; set; }
        public int? BranchId { get; set; }
        public string? Branch { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string? CurrencyType { get; set; }
        public DateTime Date { get; set; }
        public double BaseSalary { get; set; }
        public double AdvanceDeduction { get; set; }
        public double NetSalary { get; set; }
        public Guid? PayedBy { get; set; }
        public string? PayedByUserName { get; set; }
        public bool IsPayed { get; set; } = false;

    }
}
