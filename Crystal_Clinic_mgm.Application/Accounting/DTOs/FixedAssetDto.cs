namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class FixedAssetDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal ResidualValue { get; set; }
        public int UsefulLifeMonths { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal CurrentValue { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class CreateFixedAssetDto
    {
        public string AssetCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal ResidualValue { get; set; }
        public int UsefulLifeMonths { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int? BranchId { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateFixedAssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal ResidualValue { get; set; }
        public int UsefulLifeMonths { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class DepreciationScheduleDto
    {
        public int FixedAssetId { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public DateTime AcquisitionDate { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal ResidualValue { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
    }

    public class FixedAssetDepreciationDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public DateTime AcquisitionDate { get; set; }
        public decimal MonthlyDepreciation { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal CurrentNetValue { get; set; }
    }
}

