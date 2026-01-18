using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IFinancialService
    {
        Task<Result> CreateJournalEntryAsync(JournalEntryRequest request, CancellationToken cancellationToken = default);
        Task<Result> PostJournalEntryAsync(int journalEntryId, CancellationToken cancellationToken = default);
        Task<JournalEntryDto?> GetJournalEntryAsync(int journalEntryId, CancellationToken cancellationToken = default);
    }

    public class JournalEntryRequest
    {
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public string ReferenceType { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public IEnumerable<JournalEntryLineRequest> Lines { get; set; } = new List<JournalEntryLineRequest>();
    }

    public class JournalEntryLineRequest
    {
        public string AccountCode { get; set; } = string.Empty; // Chart of accounts code
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int? CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
    }
}