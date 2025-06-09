namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList
{
    public class GetContractDetailsListModel
    {
        public int ID { get; set; }
        public int EmployeeProfileId { get; set; }
        public string? EmployeeName { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractType { get; set; } = string.Empty;
        public int PositionTitleId { get; set; }
        public string PositionTitle { get; set; } = string.Empty;
        public int Branchid { get; set; }
        public string Branch { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } = null;
        public bool IsActive { get; set; }
        public string Remarks { get; set; } = string.Empty;

    }
}
