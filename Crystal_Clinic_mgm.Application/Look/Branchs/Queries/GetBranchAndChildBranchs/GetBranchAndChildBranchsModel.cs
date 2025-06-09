namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentAndChildDepartments
{
    public class GetBranchAndChildBranchsModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string OfficialEmail { get; set; } = string.Empty;
        public string OfficialContact { get; set; } = string.Empty;
        public string FourDigitNumber { get; set; } = string.Empty;
        public string Block { get; set; } = string.Empty;

    }
}
