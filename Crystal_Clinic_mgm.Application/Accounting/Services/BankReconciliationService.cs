using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class BankReconciliationService(IMediator mediator) : IBankReconciliationService
    {
        public async Task<Result> UploadBankStatementAsync(CreateBankStatementImportDto dto)
        {
            var command = new UploadBankStatementCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> AutoMatchTransactionsAsync(int bankStatementImportId, decimal amountTolerance = 0.01m, int dateTolerance = 5)
        {
            var command = new AutoMatchBankTransactionsCommand
            {
                BankStatementImportId = bankStatementImportId,
                AmountTolerance = amountTolerance,
                DateTolerance = dateTolerance
            };
            return await mediator.Send(command);
        }

        public async Task<Result> ManualMatchTransactionAsync(CreateBankMatchDto dto)
        {
            var command = new ManualMatchBankTransactionCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> CloseBankReconciliationAsync(int bankStatementImportId)
        {
            var command = new CloseBankReconciliationCommand { BankStatementImportId = bankStatementImportId };
            return await mediator.Send(command);
        }

        public async Task<Result> GetBankStatementImportAsync(int importId)
        {
            var query = new GetBankStatementImportQuery { ImportId = importId };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAllBankStatementImportsAsync(int? bankAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)
        {
            var query = new GetAllBankStatementImportsQuery
            {
                BankAccountId = bankAccountId,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize
            };
            return await mediator.Send(query);
        }

        public async Task<Result> GetReconciliationSummaryAsync(int bankStatementImportId)
        {
            var query = new GetBankReconciliationSummaryQuery { BankStatementImportId = bankStatementImportId };
            return await mediator.Send(query);
        }

        public async Task<Result> GetUnmatchedTransactionsAsync(int bankStatementImportId)
        {
            var query = new GetUnmatchedBankTransactionsQuery { BankStatementImportId = bankStatementImportId };
            return await mediator.Send(query);
        }
    }
}
