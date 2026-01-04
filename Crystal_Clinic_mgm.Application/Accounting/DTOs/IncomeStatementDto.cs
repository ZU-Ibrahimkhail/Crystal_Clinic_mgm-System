namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class IncomeStatementDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? BranchId { get; set; }

        public decimal TotalRevenue { get; set; }
        public List<RevenueLineDto> RevenueLines { get; set; } = new();

        public decimal TotalCostOfGoodsSold { get; set; }
        public List<CostLineDto> CostLines { get; set; } = new();

        public decimal GrossProfit { get; set; }

        public decimal TotalOperatingExpenses { get; set; }
        public List<ExpenseLineDto> OperatingExpenseLines { get; set; } = new();

        public decimal OperatingIncome { get; set; }

        public decimal TotalOtherIncome { get; set; }
        public List<OtherIncomeLineDto> OtherIncomeLines { get; set; } = new();

        public decimal TotalOtherExpenses { get; set; }
        public List<OtherExpenseLineDto> OtherExpenseLines { get; set; } = new();

        public decimal NetIncome { get; set; }

        public string? Currency { get; set; }
    }

    public class RevenueLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class CostLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class ExpenseLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class OtherIncomeLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class OtherExpenseLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
