namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class ForecastSnapshot : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Scenario { get; set; } = string.Empty;
        public DateTime SnapshotDate { get; set; }
        public int HorizonMonths { get; set; }
        public DateTime ForecastStartDate { get; set; }
        public DateTime ForecastEndDate { get; set; }
        public decimal ProjectedCashBalance { get; set; }
        public decimal ProjectedAccountsReceivable { get; set; }
        public decimal ProjectedAccountsPayable { get; set; }
        public decimal ProjectedNetIncome { get; set; }
        public ForecastStatus Status { get; set; } = ForecastStatus.Draft;
        public int? CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? Notes { get; set; }

        public ICollection<ForecastLine> Lines { get; set; } = new List<ForecastLine>();
    }

    public class ForecastLine : AuditableEntity
    {
        public int Id { get; set; }
        public int ForecastSnapshotId { get; set; }
        public ForecastSnapshot ForecastSnapshot { get; set; } = null!;
        public int ForecastMonth { get; set; }
        public DateTime ForecastDate { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public string MetricName { get; set; } = string.Empty;
        public decimal ProjectedValue { get; set; }
        public decimal? VariancePercentage { get; set; }
        public string? Notes { get; set; }
    }

    public enum ForecastStatus
    {
        Draft,
        Submitted,
        Approved,
        Archived
    }

    public enum ForecastMetricType
    {
        CashFlow,
        AccountsReceivable,
        AccountsPayable,
        Revenue,
        Expense,
        NetIncome,
        InventoryValue
    }
}
