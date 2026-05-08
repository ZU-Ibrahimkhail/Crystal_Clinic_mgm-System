using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Journal Entries
    public class GetAllJournalEntriesQuery : IRequest<Result>
    {
        public JournalEntryStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? EquityTransactionId { get; set; }
        public int? ExpenseId { get; set; }
        public int? PaymentId { get; set; }
        public int? SalesReceiptId { get; set; }
        public int? ReceiptId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllJournalEntriesQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllJournalEntriesQuery, Result>
    {
        public async Task<Result> Handle(GetAllJournalEntriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.JournalEntries
                    .Where(j => !j.IsDeleted);

                if (request.Status.HasValue)
                    query = query.Where(j => j.Status == request.Status);

                if (request.FromDate.HasValue)
                    query = query.Where(j => j.EntryDate >= request.FromDate);

                if (request.ToDate.HasValue)
                    query = query.Where(j => j.EntryDate <= request.ToDate);

                if (request.EquityTransactionId.HasValue)
                    query = query.Where(j => j.EquityTransactionId == request.EquityTransactionId);

                if (request.ExpenseId.HasValue)
                    query = query.Where(j => j.ExpenseId == request.ExpenseId);

                if (request.PaymentId.HasValue)
                    query = query.Where(j => j.PaymentId == request.PaymentId);

                if (request.SalesReceiptId.HasValue)
                    query = query.Where(j => j.SalesReceiptId == request.SalesReceiptId);

                if (request.ReceiptId.HasValue)
                    query = query.Where(j => j.ReceiptId == request.ReceiptId);

                var totalCount = await query.CountAsync(cancellationToken);

                var entries = await query
                    .OrderByDescending(j => j.EntryDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(j => j.JournalEntryLines)
                    .Select(j => new JournalEntryDto
                    {
                        Id = j.Id,
                        EntryNumber = j.EntryNumber,
                        EntryDate = j.EntryDate,
                        Description = j.Description,
                        Status = j.Status,
                        ReferenceNumber = j.ReferenceNumber,
                        ReferenceType = j.ReferenceType,
                        BranchId = j.BranchId,
                        EquityTransactionId = j.EquityTransactionId,
                        ExpenseId = j.ExpenseId,
                        PaymentId = j.PaymentId,
                        SalesReceiptId = j.SalesReceiptId,
                        ReceiptId = j.ReceiptId,
                        Lines = j.JournalEntryLines.Select(l => new JournalEntryLineDto
                        {
                            Id = l.Id,
                            ChartOfAccountId = l.ChartOfAccountId,
                            Description = l.Description,
                            DebitAmount = l.DebitAmount,
                            CreditAmount = l.CreditAmount,
                            CurrencyId = l.CurrencyId,
                            ExchangeRate = l.ExchangeRate,
                            AmountInBaseCurrency = l.AmountInBaseCurrency
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);

                var result = new
                {
                    entries,
                    totalCount,
                    pageNumber = request.PageNumber,
                    pageSize = request.PageSize
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Journal Entries: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Journal Entry By Id
    public class GetJournalEntryByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetJournalEntryByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetJournalEntryByIdQuery, Result>
    {
        public async Task<Result> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entry = await context.JournalEntries
                    .Include(j => j.JournalEntryLines)
                    .ThenInclude(i => i.ChartOfAccount)
                    .FirstOrDefaultAsync(j => j.Id == request.Id && !j.IsDeleted, cancellationToken);

                if (entry == null)
                    return Result.Fail("Journal Entry not found.");

                var dto = new JournalEntryDto
                {
                    Id = entry.Id,
                    EntryNumber = entry.EntryNumber,
                    EntryDate = entry.EntryDate,
                    Description = entry.Description,
                    Status = entry.Status,
                    ReferenceNumber = entry.ReferenceNumber,
                    ReferenceType = entry.ReferenceType,
                    BranchId = entry.BranchId,
                    Attachment = entry.Attachment,
                    Lines = entry.JournalEntryLines
                        .Where(l => !l.IsDeleted)
                        .Select(l => new JournalEntryLineDto
                        {
                            AccountCode = l.ChartOfAccount.AccountCode,
                            AccountName = l.ChartOfAccount.AccountName,
                            Id = l.Id,
                            ChartOfAccountId = l.ChartOfAccountId,
                            Description = l.Description,
                            DebitAmount = l.DebitAmount,
                            CreditAmount = l.CreditAmount,
                            CurrencyId = l.CurrencyId,
                            ExchangeRate = l.ExchangeRate,
                            AmountInBaseCurrency = l.AmountInBaseCurrency
                        }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Journal Entry: {ex.Message}");
            }
        }
    }
    #endregion
}
