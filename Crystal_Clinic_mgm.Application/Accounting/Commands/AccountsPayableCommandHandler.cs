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
                var payable = new AccountsPayable
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
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
