using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public string BudgetName { get; set; } = string.Empty;
        public int FiscalYear { get; set; }
        public BudgetStatus Status { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? BranchId { get; set; }
        public List<BudgetLineDto> Lines { get; set; } = new();
    }

    public class BudgetLineDto
    {
        public int Id { get; set; }
        public int ChartOfAccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public int PeriodId { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
    }

    public class CreateBudgetDto
    {
        public string BudgetName { get; set; } = string.Empty;
        public int FiscalYear { get; set; }
        public int? BranchId { get; set; }
        public List<CreateBudgetLineDto> Lines { get; set; } = new();
    }

    public class CreateBudgetLineDto
    {
        public int ChartOfAccountId { get; set; }
        public int PeriodId { get; set; }
        public decimal BudgetedAmount { get; set; }
    }

    public class BudgetVarianceDto
    {
        public int BudgetLineId { get; set; }
        public int ChartOfAccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public decimal VariancePercentage { get; set; }
    }
}
