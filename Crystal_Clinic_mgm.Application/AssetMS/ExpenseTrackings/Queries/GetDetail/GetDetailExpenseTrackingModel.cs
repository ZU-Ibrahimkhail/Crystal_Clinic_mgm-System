using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail
{
    public class GetExpenseTrackingDetailModel
    {

        public int Id { get; set; }
        public int CurrencyTypeId { get; set; }
        public string? CurrencyType { get; set; }
        public int ExpenseTypeId { get; set; }
        public string? ExpenseType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public Guid? MainAccountId { get; set; }
        public string? MainAccountUserName { get; set; }
        public string? CreatedByUserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
