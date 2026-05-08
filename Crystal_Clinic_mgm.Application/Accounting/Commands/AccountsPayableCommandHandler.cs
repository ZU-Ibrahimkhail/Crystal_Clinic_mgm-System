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
    #region Create Accounts Payable
    public class CreateAccountsPayableCommand : IRequest<Result>
    {
        public CreateAccountsPayableDto Dto { get; set; } = null!;
    }

    public class CreateAccountsPayableCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateAccountsPayableCommand, Result>
    {
        public async Task<Result> Handle(CreateAccountsPayableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var companyProfile = await context.CompanyProfile
                    .FirstOrDefaultAsync(cancellationToken);

                if (companyProfile == null)
                    return Result.Fail("Company profile not configured. Please set up the company profile first.");

                int debitAccountId;
                if (request.Dto.ChartOfAccountId.HasValue)
                {
                    debitAccountId = request.Dto.ChartOfAccountId.Value;
                }
                else
                {
                    debitAccountId = companyProfile.PurchaseExpenseAccountId;
                }

                var debitAccount = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == debitAccountId && c.IsActive && !c.IsDeleted, cancellationToken);

                if (debitAccount == null)
                    return Result.Fail("Expense account not found or inactive.");

                var apAccount = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == companyProfile.AccountsPayableAccountId && c.IsActive && !c.IsDeleted, cancellationToken);

                if (apAccount == null)
                    return Result.Fail("Accounts Payable account not configured in company profile.");

                var invoiceNumber = GenerateInvoiceNumber();
                decimal exchangeRate = request.Dto.CurrencyRate > 0 ? (decimal)request.Dto.CurrencyRate : 1m;
                decimal amountInBase = request.Dto.InvoiceAmount * exchangeRate;

                var payable = new AccountsPayable
                {
                    InvoiceNumber = invoiceNumber,
                    EmployeeId = request.Dto.EmployeeId,
                    InvoiceDate = request.Dto.InvoiceDate,
                    DueDate = request.Dto.DueDate,
                    InvoiceAmount = request.Dto.InvoiceAmount,
                    BalanceAmount = request.Dto.InvoiceAmount,
                    Status = APStatus.Draft,
                    Type = request.Dto.Type,
                    ChartOfAccountId = request.Dto.ChartOfAccountId,
                    CurrencyRate = request.Dto.CurrencyRate,
                    Attachment = request.Dto.Attachment,
                    Description = request.Dto.Description,
                    Reference = request.Dto.Reference,
                    BranchId = request.Dto.BranchId,
                    CurrencyId = request.Dto.CurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.AccountsPayables.Add(payable);
                await context.SaveChangesAsync(cancellationToken);

                var journalEntryNumber = $"JE-AP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
                var journalEntry = new JournalEntry
                {
                    EntryNumber = journalEntryNumber,
                    EntryDate = request.Dto.InvoiceDate,
                    Description = $"Accounts Payable - {invoiceNumber}" + (string.IsNullOrEmpty(request.Dto.Description) ? "" : $": {request.Dto.Description}"),
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = invoiceNumber,
                    ReferenceType = "AccountsPayable",
                    BranchId = request.Dto.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                var debitLine = new JournalEntryLine
                {
                    ChartOfAccountId = debitAccountId,
                    Description = $"Expense - {invoiceNumber}",
                    DebitAmount = request.Dto.InvoiceAmount,
                    CreditAmount = 0,
                    CurrencyId = request.Dto.CurrencyId,
                    ExchangeRate = exchangeRate,
                    AmountInBaseCurrency = amountInBase,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                var creditLine = new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                    Description = $"Accounts Payable - {invoiceNumber}",
                    DebitAmount = 0,
                    CreditAmount = request.Dto.InvoiceAmount,
                    CurrencyId = request.Dto.CurrencyId,
                    ExchangeRate = exchangeRate,
                    AmountInBaseCurrency = amountInBase,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                journalEntry.JournalEntryLines.Add(debitLine);
                journalEntry.JournalEntryLines.Add(creditLine);

                context.JournalEntries.Add(journalEntry);
                await context.SaveChangesAsync(cancellationToken);

                var debitLedger = new GeneralLedger
                {
                    ChartOfAccountId = debitAccountId,
                    JournalEntryId = journalEntry.Id,
                    BranchId = request.Dto.BranchId,
                    TransactionDate = request.Dto.InvoiceDate,
                    Description = $"Expense - {invoiceNumber}",
                    DebitAmount = request.Dto.InvoiceAmount,
                    CreditAmount = 0,
                    Balance = request.Dto.InvoiceAmount,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                var creditLedger = new GeneralLedger
                {
                    ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                    JournalEntryId = journalEntry.Id,
                    BranchId = request.Dto.BranchId,
                    TransactionDate = request.Dto.InvoiceDate,
                    Description = $"Accounts Payable - {invoiceNumber}",
                    DebitAmount = 0,
                    CreditAmount = request.Dto.InvoiceAmount,
                    Balance = request.Dto.InvoiceAmount,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.GeneralLedgers.Add(debitLedger);
                context.GeneralLedgers.Add(creditLedger);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(payable.Id, $"Bill {payable.InvoiceNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Accounts Payable: {ex.Message}");
            }
        }

        private string GenerateInvoiceNumber()
        {
            return $"AP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Approve Accounts Payable
    public class ApproveAccountsPayableCommand : IRequest<Result>
    {
        public int AccountsPayableId { get; set; }
    }

    public class ApproveAccountsPayableCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ApproveAccountsPayableCommand, Result>
    {
        public async Task<Result> Handle(ApproveAccountsPayableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payable = await context.AccountsPayables
                    .FirstOrDefaultAsync(a => a.Id == request.AccountsPayableId && !a.IsDeleted, cancellationToken);

                if (payable == null)
                    return Result.Fail("Accounts Payable not found.");

                payable.Status = APStatus.Approve;
                payable.ModifiedBy = loggedInUser.Id;
                payable.ModifiedOn = DateTime.UtcNow;

                context.AccountsPayables.Update(payable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Accounts Payable approved successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error approving Accounts Payable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Mark Accounts Payable For Payment
    public class MarkAccountsPayableForPaymentCommand : IRequest<Result>
    {
        public int AccountsPayableId { get; set; }
    }

    public class MarkAccountsPayableForPaymentCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<MarkAccountsPayableForPaymentCommand, Result>
    {
        public async Task<Result> Handle(MarkAccountsPayableForPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payable = await context.AccountsPayables
                    .FirstOrDefaultAsync(a => a.Id == request.AccountsPayableId && !a.IsDeleted, cancellationToken);

                if (payable == null)
                    return Result.Fail("Accounts Payable not found.");

                payable.ModifiedBy = loggedInUser.Id;
                payable.ModifiedOn = DateTime.UtcNow;

                context.AccountsPayables.Update(payable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Bill marked for payment successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error marking bill for payment: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Payment
    public class RecordPaymentCommand : IRequest<Result>
    {
        public CreatePaymentDto Dto { get; set; } = null!;
    }

    public class RecordPaymentCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordPaymentCommand, Result>
    {
        public async Task<Result> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payable = await context.AccountsPayables
                    .FirstOrDefaultAsync(a => a.Id == request.Dto.AccountsPayableId && !a.IsDeleted, cancellationToken);

                if (payable == null)
                    return Result.Fail("Accounts Payable not found.");

                if (request.Dto.AmountPaid > payable.BalanceAmount)
                    return Result.Fail("Payment amount exceeds outstanding balance.");

                decimal exchangeRate;
                int currencyId = request.Dto.CurrencyId ?? payable.CurrencyId ?? 1;
                const int BaseCurrencyId = 1;

                if (request.Dto.ExchangeRate.HasValue && request.Dto.ExchangeRate.Value > 0)
                {
                    exchangeRate = request.Dto.ExchangeRate.Value;
                }
                else
                {
                    exchangeRate = await context.GetExchangeRate(currencyId, BaseCurrencyId, cancellationToken);
                }

                var payment = new Payment
                {
                    AccountsPayableId = request.Dto.AccountsPayableId,
                    PaymentNumber = GeneratePaymentNumber(),
                    PaymentDate = request.Dto.PaymentDate,
                    AmountPaid = request.Dto.AmountPaid,
                    PaymentMethodId = request.Dto.PaymentMethodId,
                    CurrencyId = currencyId,
                    ExchangeRate = exchangeRate,
                    AmountInBaseCurrency = request.Dto.AmountPaid * exchangeRate,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                payable.PaidAmount += request.Dto.AmountPaid;
                payable.BalanceAmount -= request.Dto.AmountPaid;
                payable.Status = payable.BalanceAmount == 0 ? APStatus.Paid : APStatus.PartiallyPaid;

                context.Payments.Add(payment);
                context.AccountsPayables.Update(payable);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(payment.Id, "Payment recorded successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording payment: {ex.Message}");
            }
        }

        private string GeneratePaymentNumber()
        {
            return $"PMT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion
}
