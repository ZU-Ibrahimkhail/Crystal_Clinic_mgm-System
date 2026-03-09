using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class ShareholderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal OwnershipPercentage { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalDrawings { get; set; }
        public decimal NetEquity { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Attachment { get; set; }
    }

    public class CreateShareholderDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal OwnershipPercentage { get; set; }
        public decimal TotalInvestment { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Attachment { get; set; }
    }

    public class UpdateShareholderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal OwnershipPercentage { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class EquityTransactionDto
    {
        public int Id { get; set; }
        public int ShareholderId { get; set; }
        public string ShareholderName { get; set; } = string.Empty;
        public EquityTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }

    public class CreateEquityTransactionDto
    {
        public int ShareholderId { get; set; }
        public EquityTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }

    public class ShareholderEquitySummaryDto
    {
        public int ShareholderId { get; set; }
        public string ShareholderName { get; set; } = string.Empty;
        public decimal OwnershipPercentage { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalDrawings { get; set; }
        public decimal NetEquity { get; set; }
        public List<EquityTransactionDto> Transactions { get; set; } = new();
    }

    public class EquityReportDto
    {
        public DateTime AsOfDate { get; set; }
        public decimal TotalEquity { get; set; }
        public List<ShareholderEquitySummaryDto> Shareholders { get; set; } = new();
    }
}
