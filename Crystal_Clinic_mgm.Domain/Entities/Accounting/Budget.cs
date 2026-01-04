using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class Budget : AuditableEntity
    {
        public int Id { get; set; }
        public string BudgetName { get; set; } = string.Empty;
        public int FiscalYear { get; set; }
        public BudgetStatus Status { get; set; } = BudgetStatus.Draft;
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public ICollection<BudgetLine> BudgetLines { get; set; } = new List<BudgetLine>();
    }
}
