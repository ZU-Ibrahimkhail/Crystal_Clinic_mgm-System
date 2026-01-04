using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class BudgetLine : AuditableEntity
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public Budget Budget { get; set; } = null!;
        public int ChartOfAccountId { get; set; }
        public ChartOfAccounts ChartOfAccount { get; set; } = null!;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int PeriodId { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; } = 0;
        public decimal Variance { get; set; } = 0;
    }
}
