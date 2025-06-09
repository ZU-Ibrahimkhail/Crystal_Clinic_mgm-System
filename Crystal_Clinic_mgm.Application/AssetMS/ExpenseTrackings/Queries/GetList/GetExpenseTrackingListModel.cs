using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetList
{
    public class GetExpenseTrackingListModel
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
    }
}
