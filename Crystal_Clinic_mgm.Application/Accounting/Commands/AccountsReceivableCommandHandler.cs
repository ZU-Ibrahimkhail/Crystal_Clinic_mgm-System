using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Accounts Receivable
    public class CreateAccountsReceivableCommand : IRequest<Result>
    {
        public CreateAccountsReceivableDto Dto { get; set; } = null!;
    }

    public class CreateAccountsReceivableCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateAccountsReceivableCommand, Result>
    {
        public async Task<Result> Handle(CreateAccountsReceivableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await context.Patient
                    .FirstOrDefaultAsync(p => p.patientId == request.Dto.CustomerId, cancellationToken);

                if (customer == null)
                    return Result.Fail("Customer not found.");

                var invoiceNumber = GenerateInvoiceNumber();
                var receivable = new AccountsReceivable
                {
                    InvoiceNumber = invoiceNumber,
                    CustomerId = request.Dto.CustomerId,
                    InvoiceDate = request.Dto.InvoiceDate,
                    DueDate = request.Dto.DueDate,
                    InvoiceAmount = request.Dto.InvoiceAmount,
                    BalanceAmount = request.Dto.InvoiceAmount,
                    Status = ARStatus.Open,
                    ChartOfAccountId = request.Dto.ChartOfAccountId,
                    BranchId = request.Dto.BranchId,
                    CurrencyId = request.Dto.CurrencyId,
                    VisitId = request.Dto.VisitId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.AccountsReceivables.Add(receivable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(receivable.Id, $"Invoice {invoiceNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Accounts Receivable: {ex.Message}");
            }
        }

        private string GenerateInvoiceNumber()
        {
            return $"AR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Issue Accounts Receivable
    public class IssueAccountsReceivableCommand : IRequest<Result>
    {
        public int AccountsReceivableId { get; set; }
    }

    public class IssueAccountsReceivableCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<IssueAccountsReceivableCommand, Result>
    {
        public async Task<Result> Handle(IssueAccountsReceivableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var receivable = await context.AccountsReceivables
                    .FirstOrDefaultAsync(a => a.Id == request.AccountsReceivableId && !a.IsDeleted, cancellationToken);

                if (receivable == null)
                    return Result.Fail("Accounts Receivable not found.");

                receivable.Status = ARStatus.Open;
                receivable.ModifiedBy = loggedInUser.Id;
                receivable.ModifiedOn = DateTime.UtcNow;

                context.AccountsReceivables.Update(receivable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Accounts Receivable issued successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error issuing Accounts Receivable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Receipt
    public class RecordReceiptCommand : IRequest<Result>
    {
        public CreateReceiptDto Dto { get; set; } = null!;
    }

    public class RecordReceiptCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordReceiptCommand, Result>
    {
        public async Task<Result> Handle(RecordReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var receivable = await context.AccountsReceivables
                    .FirstOrDefaultAsync(a => a.Id == request.Dto.AccountsReceivableId && !a.IsDeleted, cancellationToken);

                if (receivable == null)
                    return Result.Fail("Accounts Receivable not found.");

                if (request.Dto.AmountReceived > receivable.BalanceAmount)
                    return Result.Fail("Receipt amount exceeds outstanding balance.");

                decimal exchangeRate;
                int currencyId = request.Dto.CurrencyId ?? receivable.CurrencyId ?? 1;
                const int BaseCurrencyId = 1;

                if (request.Dto.ExchangeRate.HasValue && request.Dto.ExchangeRate.Value > 0)
                {
                    exchangeRate = request.Dto.ExchangeRate.Value;
                }
                else
                {
                    exchangeRate = await context.GetExchangeRate(currencyId, BaseCurrencyId, cancellationToken);
                }

                var receipt = new Receipt
                {
                    AccountsReceivableId = request.Dto.AccountsReceivableId,
                    ReceiptNumber = GenerateReceiptNumber(),
                    ReceiptDate = request.Dto.ReceiptDate,
                    AmountReceived = request.Dto.AmountReceived,
                    PaymentMethodId = request.Dto.PaymentMethodId,
                    CurrencyId = currencyId,
                    ExchangeRate = exchangeRate,
                    AmountInBaseCurrency = request.Dto.AmountReceived * exchangeRate,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                receivable.PaidAmount += request.Dto.AmountReceived;
                receivable.BalanceAmount -= request.Dto.AmountReceived;
                receivable.Status = receivable.BalanceAmount == 0 ? ARStatus.Paid : ARStatus.PartiallyPaid;

                context.Receipts.Add(receipt);
                context.AccountsReceivables.Update(receivable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(receipt.Id, "Receipt recorded successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording Receipt: {ex.Message}");
            }
        }

        private string GenerateReceiptNumber()
        {
            return $"RCP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion
}
