using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string EntryNumber { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public JournalEntryStatus Status { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string ReferenceType { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; } = new();
    }

    public class JournalEntryLineDto
    {
        public int Id { get; set; }
        public int ChartOfAccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int? CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal AmountInBaseCurrency { get; set; }
    }

    public class CreateJournalEntryDto
    {
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public string ReferenceType { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public List<CreateJournalEntryLineDto> Lines { get; set; } = new();
    }

    public class CreateJournalEntryLineDto
    {
        public int ChartOfAccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int? CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
    }

    public class PostJournalEntryDto
    {
        public int JournalEntryId { get; set; }
    }
}
