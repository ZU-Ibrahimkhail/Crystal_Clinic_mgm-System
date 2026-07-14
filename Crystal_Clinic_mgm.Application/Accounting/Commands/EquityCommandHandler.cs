using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Shareholder
    public class CreateShareholderCommand : IRequest<Result>
    {
        public CreateShareholderDto Dto { get; set; } = null!;
    }

    public class CreateShareholderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateShareholderCommand, Result>
    {
        public async Task<Result> Handle(CreateShareholderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var shareholder = new Shareholder
                {
                    Name = request.Dto.Name,
                    OwnershipPercentage = request.Dto.OwnershipPercentage,
                    TotalInvestment = request.Dto.TotalInvestment,
                    TotalDrawings = 0,
                    ContactInfo = request.Dto.ContactInfo,
                    Email = request.Dto.Email,
                    IsActive = true,
                    Attachment = request.Dto.Attachment,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.Shareholders.Add(shareholder);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(shareholder.Id, "Shareholder created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Shareholder: {ex.Message}");
            }
        }
    }
    #endregion

    #region Update Shareholder
    public class UpdateShareholderCommand : IRequest<Result>
    {
        public UpdateShareholderDto Dto { get; set; } = null!;
    }

    public class UpdateShareholderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateShareholderCommand, Result>
    {
        public async Task<Result> Handle(UpdateShareholderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var shareholder = await context.Shareholders
                    .FirstOrDefaultAsync(s => s.Id == request.Dto.Id && !s.IsDeleted, cancellationToken);

                if (shareholder == null)
                    return Result.Fail("Shareholder not found.");

                shareholder.Name = request.Dto.Name;
                shareholder.OwnershipPercentage = request.Dto.OwnershipPercentage;
                shareholder.ContactInfo = request.Dto.ContactInfo;
                shareholder.Email = request.Dto.Email;
                shareholder.IsActive = request.Dto.IsActive;
                shareholder.ModifiedBy = loggedInUser.Id;
                shareholder.ModifiedOn = DateTime.UtcNow;

                context.Shareholders.Update(shareholder);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Shareholder updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Shareholder: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Equity Transaction
    public class RecordEquityTransactionCommand : IRequest<Result>
    {
        public CreateEquityTransactionDto Dto { get; set; } = null!;
    }

    public class RecordEquityTransactionCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordEquityTransactionCommand, Result>
    {
        public async Task<Result> Handle(RecordEquityTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var shareholder = await context.Shareholders
                    .FirstOrDefaultAsync(s => s.Id == request.Dto.ShareholderId && !s.IsDeleted, cancellationToken);

                if (shareholder == null)
                    return Result.Fail("Shareholder not found.");

                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (companyProfile == null)
                    return Result.Fail("Company profile not configured.");

                var transaction = new EquityTransaction
                {
                    ShareholderId = request.Dto.ShareholderId,
                    Type = request.Dto.Type,
                    Amount = request.Dto.Amount,
                    TransactionDate = request.Dto.TransactionDate,
                    Description = request.Dto.Description,
                    Reference = request.Dto.Reference,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                if (request.Dto.Type == EquityTransactionType.Investment)
                {
                    shareholder.TotalInvestment += request.Dto.Amount;
                }
                else if (request.Dto.Type == EquityTransactionType.Drawing)
                {
                    shareholder.TotalDrawings += request.Dto.Amount;
                }

                shareholder.ModifiedBy = loggedInUser.Id;
                shareholder.ModifiedOn = DateTime.UtcNow;

                context.EquityTransactions.Add(transaction);
                context.Shareholders.Update(shareholder);
                await context.SaveChangesAsync(cancellationToken);

                var je = new JournalEntry
                {
                    EntryNumber = $"JE-EQ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = request.Dto.TransactionDate,
                    Description = $"Equity {(request.Dto.Type == EquityTransactionType.Investment ? "Investment" : "Drawing")} - {shareholder.Name}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = request.Dto.Reference ?? $"EQ-{transaction.Id}",
                    ReferenceType = request.Dto.Type == EquityTransactionType.Investment ? "Equity Investment" : "Equity Drawing",
                    EquityTransactionId = transaction.Id,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                if (!companyProfile.EquityAccountId.HasValue)
                    return Result.Fail("Equity account not configured in company profile.");

                if (request.Dto.Type == EquityTransactionType.Investment)
                {
                    je.JournalEntryLines.Add(new JournalEntryLine
                    {
                        ChartOfAccountId = companyProfile.CashAccountId,
                        Description = $"Cash received from {shareholder.Name}",
                        DebitAmount = request.Dto.Amount,
                        Status = JournalEntryStatus.Posted,
                        CreditAmount = 0,
                        EquityTransactionId = transaction.Id,
                        AmountInBaseCurrency = request.Dto.Amount,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    });

                    je.JournalEntryLines.Add(new JournalEntryLine
                    {
                        ChartOfAccountId = companyProfile.EquityAccountId.Value,
                        Description = $"Capital contribution from {shareholder.Name}",
                        DebitAmount = 0,
                        CreditAmount = request.Dto.Amount,
                        Status = JournalEntryStatus.Posted,
                        AmountInBaseCurrency = request.Dto.Amount,
                        EquityTransactionId = transaction.Id,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    });
                }
                else if (request.Dto.Type == EquityTransactionType.Drawing)
                {
                    je.JournalEntryLines.Add(new JournalEntryLine
                    {
                        ChartOfAccountId = companyProfile.EquityAccountId.Value,
                        Description = $"Dividend/Drawing paid to {shareholder.Name}",
                        DebitAmount = request.Dto.Amount,
                        CreditAmount = 0,
                        AmountInBaseCurrency = request.Dto.Amount,
                        Status = JournalEntryStatus.Posted,
                        EquityTransactionId = transaction.Id,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    });

                    je.JournalEntryLines.Add(new JournalEntryLine
                    {
                        ChartOfAccountId = companyProfile.CashAccountId,
                        Description = $"Cash paid to {shareholder.Name}",
                        DebitAmount = 0,
                        CreditAmount = request.Dto.Amount,
                        Status = JournalEntryStatus.Posted,
                        EquityTransactionId = transaction.Id,
                        AmountInBaseCurrency = request.Dto.Amount,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                context.JournalEntries.Add(je);
                await context.SaveChangesAsync(cancellationToken);
                await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                return Result.Success(transaction.Id, "Equity transaction recorded successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording Equity transaction: {ex.Message}");
            }
        }

    }
    #endregion
}
