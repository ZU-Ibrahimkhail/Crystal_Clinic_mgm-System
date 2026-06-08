namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    public class UnbilledVisitItemsDto
    {
        public int VisitId { get; set; }
        public List<UnbilledMedicationDto> Medications { get; set; } = new();
        public List<UnbilledKitDto> Kits { get; set; } = new();
        public List<UnbilledServiceDto> Services { get; set; } = new();
        public decimal TotalUnbilledAmount { get; set; }
    }

    public class UnbilledMedicationDto
    {
        public int ItemId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public int AvailableQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int StockId { get; set; }
    }

    public class UnbilledKitDto
    {
        public int KitId { get; set; }
        public string KitName { get; set; } = string.Empty;
        public int AvailableQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class UnbilledServiceDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int AvailableSessionCount { get; set; }
        public decimal PricePerSession { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
