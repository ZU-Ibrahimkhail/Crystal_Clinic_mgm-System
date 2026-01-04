using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class ForecastSnapshotDto
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
        public ForecastStatus Status { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? Notes { get; set; }
        public List<ForecastLineDto> Lines { get; set; } = new();
    }

    public class CreateForecastSnapshotDto
    {
        public string Name { get; set; } = string.Empty;
        public string Scenario { get; set; } = string.Empty;
        public int HorizonMonths { get; set; }
        public decimal? GrowthPercentage { get; set; }
        public decimal? CollectionDaysChange { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateForecastSnapshotDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Scenario { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class ForecastLineDto
    {
        public int Id { get; set; }
        public int ForecastMonth { get; set; }
        public DateTime ForecastDate { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public string MetricName { get; set; } = string.Empty;
        public decimal ProjectedValue { get; set; }
        public decimal? VariancePercentage { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateForecastLineDto
    {
        public int ForecastMonth { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public string MetricName { get; set; } = string.Empty;
        public decimal ProjectedValue { get; set; }
        public decimal? VariancePercentage { get; set; }
        public string? Notes { get; set; }
    }

    public class ForecastScenarioDto
    {
        public string ScenarioName { get; set; } = string.Empty;
        public decimal GrowthPercentage { get; set; }
        public decimal CollectionDaysChange { get; set; }
        public decimal ExchangeRateChange { get; set; }
    }
}
