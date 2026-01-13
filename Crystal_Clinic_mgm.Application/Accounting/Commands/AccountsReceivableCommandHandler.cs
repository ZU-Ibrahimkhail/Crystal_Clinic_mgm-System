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
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var receivable = await context.AccountsReceivables
                    .Include(a => a.ChartOfAccount)
                    .FirstOrDefaultAsync(a => a.Id == request.Dto.AccountsReceivableId && !a.IsDeleted, cancellationToken);

                if (receivable == null)
                    return Result.Fail("Accounts Receivable not found.");

                // Validate payment amount
                if (request.Dto.Amount <= 0)
                    return Result.Fail("Payment amount must be greater than zero.");

                // Validate overpayment limit (max 50% overpayment to prevent fraud)
                var maxAllowedOverpayment = receivable.BalanceAmount * 1.5m;
                var validationAmount = request.Dto.Amount * (request.Dto.ExchangeRate ?? 1);
                if (validationAmount > maxAllowedOverpayment)
                    return Result.Fail($"Payment amount exceeds maximum allowed overpayment limit of {maxAllowedOverpayment:N2} AFN.");

                // Get exchange rate
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

                // Convert amount to base currency
                var amountInBaseCurrency = request.Dto.Amount * exchangeRate;

                // Calculate overpayment
                var overpaymentAmount = Math.Max(0, amountInBaseCurrency - receivable.BalanceAmount);
                var appliedAmount = amountInBaseCurrency - overpaymentAmount;

                // Create receipt record for amount received
                var receipt = new Receipt
                {
                    AccountsReceivableId = request.Dto.AccountsReceivableId,
                    ReceiptNumber = GenerateReceiptNumber("REC"),
                    ReceiptDate = request.Dto.ReceiptDate,
                    TransactionType = TransactionType.Receipt,
                    Amount = request.Dto.Amount,
                    PaymentMethodId = request.Dto.PaymentMethodId,
                    Reference = request.Dto.Reference ?? string.Empty,
                    CurrencyId = currencyId,
                    ExchangeRate = exchangeRate,
                    AmountInBaseCurrency = amountInBaseCurrency,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.Receipts.Add(receipt);
                await context.SaveChangesAsync(cancellationToken); // Save to get receipt.Id

                // Create refund record if there's overpayment
                Receipt? refund = null;
                if (overpaymentAmount > 0)
                {
                    refund = new Receipt
                    {
                        AccountsReceivableId = request.Dto.AccountsReceivableId,
                        ReceiptNumber = GenerateReceiptNumber("REF"),
                        ReceiptDate = request.Dto.ReceiptDate,
                        TransactionType = TransactionType.Refund,
                        Amount = overpaymentAmount / exchangeRate, // Convert back to original currency
                        PaymentMethodId = request.Dto.PaymentMethodId,
                        Reference = $"Change for {receipt.ReceiptNumber}",
                        CurrencyId = currencyId,
                        ExchangeRate = exchangeRate,
                        AmountInBaseCurrency = overpaymentAmount,
                        OriginalReceiptId = receipt.Id,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    context.Receipts.Add(refund);
                }

                // Update AR balance
                receivable.PaidAmount += appliedAmount;
                receivable.BalanceAmount -= appliedAmount;

                if (receivable.BalanceAmount <= 0)
                {
                    receivable.Status = ARStatus.Paid;
                    receivable.BalanceAmount = 0; // Ensure no negative balance
                }
                else
                {
                    receivable.Status = ARStatus.PartiallyPaid;
                }

                context.AccountsReceivables.Update(receivable);
                await context.SaveChangesAsync(cancellationToken);

                // Create journal entries
                await CreateJournalEntries(context, receipt, refund, receivable, loggedInUser.Id, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var message = overpaymentAmount > 0
                    ? $"Payment recorded. Change given: {overpaymentAmount:N2} AFN"
                    : "Payment recorded successfully.";

                return Result.Success(receipt.Id, message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Fail($"Error recording payment: {ex.Message}");
            }
        }

        private string GenerateReceiptNumber(string suffix = "")
        {
            var baseNumber = $"RCP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            return string.IsNullOrEmpty(suffix) ? baseNumber : $"{baseNumber}-{suffix}";
        }

        private async Task CreateJournalEntries(ERP_DbContext context, Receipt receipt, Receipt? refund,
            AccountsReceivable receivable, Guid userId, CancellationToken cancellationToken)
        {
            // Get company profile for system account IDs
            var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
            if (companyProfile == null)
                throw new InvalidOperationException("Company profile not found. Please initialize the company profile first.");

            var arAccountId = companyProfile.AccountsReceivableAccountId;
            var cashAccountId = companyProfile.CashAccountId;
            var entryNumber = GenerateEntryNumber();
            var transactionDate = receipt.ReceiptDate;

            // Journal entry for receipt (money in)
            var receiptEntry = new JournalEntry
            {
                EntryNumber = $"{entryNumber}-REC",
                EntryDate = transactionDate,
                Description = $"Payment received - {receipt.ReceiptNumber}",
                Status = JournalEntryStatus.Posted,
                ReferenceNumber = receipt.ReceiptNumber,
                ReferenceType = "RECEIPT",
                BranchId = receivable.BranchId,
                ApprovedBy = userId,
                ApprovedDate = DateTime.UtcNow,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            // Debit Cash/Bank, Credit AR
            receiptEntry.JournalEntryLines.Add(new JournalEntryLine
            {
                ChartOfAccountId = arAccountId, // AR account
                Description = receipt.Reference,
                DebitAmount = 0,
                CreditAmount = receipt.AmountInBaseCurrency,
                CurrencyId = receipt.CurrencyId,
                ExchangeRate = receipt.ExchangeRate,
                AmountInBaseCurrency = receipt.AmountInBaseCurrency,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            });

            receiptEntry.JournalEntryLines.Add(new JournalEntryLine
            {
                ChartOfAccountId = cashAccountId, // Cash/Bank account
                Description = receipt.Reference,
                DebitAmount = receipt.AmountInBaseCurrency,
                CreditAmount = 0,
                CurrencyId = receipt.CurrencyId,
                ExchangeRate = receipt.ExchangeRate,
                AmountInBaseCurrency = receipt.AmountInBaseCurrency,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            });

            context.JournalEntries.Add(receiptEntry);
            await context.SaveChangesAsync(cancellationToken); // Save JE first

            // Journal entry for refund if exists (money out)
            if (refund != null)
            {
                var refundEntry = new JournalEntry
                {
                    EntryNumber = $"{entryNumber}-REF",
                    EntryDate = transactionDate,
                    Description = $"Change given - {refund.ReceiptNumber}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = refund.ReceiptNumber,
                    ReferenceType = "REFUND",
                    BranchId = receivable.BranchId,
                    ApprovedBy = userId,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

                // Debit AR, Credit Cash/Bank
                refundEntry.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = arAccountId, // AR account
                    Description = refund.Reference,
                    DebitAmount = refund.AmountInBaseCurrency,
                    CreditAmount = 0,
                    CurrencyId = refund.CurrencyId,
                    ExchangeRate = refund.ExchangeRate,
                    AmountInBaseCurrency = refund.AmountInBaseCurrency,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                });

                refundEntry.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = cashAccountId, // Cash/Bank account
                    Description = refund.Reference,
                    DebitAmount = 0,
                    CreditAmount = refund.AmountInBaseCurrency,
                    CurrencyId = refund.CurrencyId,
                    ExchangeRate = refund.ExchangeRate,
                    AmountInBaseCurrency = refund.AmountInBaseCurrency,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(refundEntry);
                await context.SaveChangesAsync(cancellationToken); // Save refund JE

                // Create GL entries for both
                await CreateGLEntries(context, receiptEntry, userId, cancellationToken);
                await CreateGLEntries(context, refundEntry, userId, cancellationToken);
            }
            else
            {
                // Create GL entries for receipt only
                await CreateGLEntries(context, receiptEntry, userId, cancellationToken);
            }
        }

        private async Task CreateGLEntries(ERP_DbContext context, JournalEntry journalEntry, Guid userId, CancellationToken cancellationToken)
        {
            foreach (var line in journalEntry.JournalEntryLines)
            {
                // Get the last balance for this account
                var lastBalance = await context.GeneralLedgers
                    .Where(gl => gl.ChartOfAccountId == line.ChartOfAccountId && !gl.IsDeleted)
                    .OrderByDescending(gl => gl.CreatedOn)
                    .Select(gl => gl.Balance)
                    .FirstOrDefaultAsync(cancellationToken);

                // Calculate new balance based on account normal balance type
                var account = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == line.ChartOfAccountId, cancellationToken);

                decimal balanceChange;
                if (account?.NormalBalance == NormalBalanceType.Debit)
                {
                    balanceChange = line.DebitAmount - line.CreditAmount;
                }
                else
                {
                    balanceChange = line.CreditAmount - line.DebitAmount;
                }

                var newBalance = lastBalance + balanceChange;

                var glEntry = new GeneralLedger
                {
                    ChartOfAccountId = line.ChartOfAccountId,
                    JournalEntryId = journalEntry.Id,
                    BranchId = journalEntry.BranchId,
                    TransactionDate = journalEntry.EntryDate,
                    Description = line.Description,
                    DebitAmount = line.DebitAmount,
                    CreditAmount = line.CreditAmount,
                    Balance = newBalance,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

                context.GeneralLedgers.Add(glEntry);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private string GenerateEntryNumber()
        {
            return $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }


    }
    #endregion
}
