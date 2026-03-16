using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FinancialService> _logger;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IAccountingRepository _accountingRepository;

        public FinancialService(
            IMediator mediator,
            ILogger<FinancialService> logger,
            ILoggedInUser currentUserService,
            IAccountingRepository accountingRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _loggedInUser = currentUserService;
            _accountingRepository = accountingRepository;
        }

        public async Task<Result> CreateJournalEntryAsync(JournalEntryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var lines = new List<CreateJournalEntryLineDto>();
                foreach (var line in request.Lines)
                {
                    var accountId = await GetAccountIdByCodeAsync(line.AccountCode, cancellationToken);
                    if (accountId == 0)
                    {
                        return Result.Fail($"Account code {line.AccountCode} not found");
                    }

                    lines.Add(new CreateJournalEntryLineDto
                    {
                        ChartOfAccountId = accountId,
                        Description = line.Description,
                        DebitAmount = line.DebitAmount,
                        CreditAmount = line.CreditAmount,
                        CurrencyId = line.CurrencyId,
                        ExchangeRate = line.ExchangeRate,
                        
                    });
                }

                var createDto = new CreateJournalEntryDto
                {
                    EntryDate = request.EntryDate,
                    Description = request.Description,
                    ReferenceNumber = request.ReferenceNumber,
                    ReferenceType = request.ReferenceType,
                    BranchId = request.BranchId,
                    Lines = lines
                };

                var command = new CreateJournalEntryCommand { Dto = createDto };
                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Created journal entry for {ReferenceType}: {ReferenceNumber}",
                        request.ReferenceType, request.ReferenceNumber);
                }
                else
                {
                    _logger.LogWarning("Failed to create journal entry: {Error}", result.Error);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating journal entry");
                return Result.Fail("Internal error occurred while creating journal entry");
            }
        }

        public async Task<Result> PostJournalEntryAsync(int journalEntryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new PostJournalEntryCommand { JournalEntryId = journalEntryId };
                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Posted journal entry {JournalEntryId}", journalEntryId);
                }
                else
                {
                    _logger.LogWarning("Failed to post journal entry {JournalEntryId}: {Error}",
                        journalEntryId, result.Error);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting journal entry {JournalEntryId}", journalEntryId);
                return Result.Fail("Internal error occurred while posting journal entry");
            }
        }

        public async Task<JournalEntryDto?> GetJournalEntryAsync(int journalEntryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetJournalEntryByIdQuery { Id = journalEntryId };
                var result = await _mediator.Send(query, cancellationToken);

                if (result.IsSuccess && result.Data is JournalEntryDto dto)
                {
                    return (JournalEntryDto?)result.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving journal entry {JournalEntryId}", journalEntryId);
                return null;
            }
        }

        private async Task<int> GetAccountIdByCodeAsync(string accountCode, CancellationToken cancellationToken)
        {
            // Query the ChartOfAccounts table to get the actual account ID
            var account = await _accountingRepository.GetAccountByCodeAsync(accountCode, cancellationToken);
            return account?.Id ?? 0;
        }
    }
}