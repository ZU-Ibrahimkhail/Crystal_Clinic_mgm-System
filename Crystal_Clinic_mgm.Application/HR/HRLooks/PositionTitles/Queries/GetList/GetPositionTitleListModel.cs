using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetList
{
    public class GetPositionTitleListModel : GeneralLookListModel
    {

        public int JobPositionId { get; set; }
        public string JobPosition { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
