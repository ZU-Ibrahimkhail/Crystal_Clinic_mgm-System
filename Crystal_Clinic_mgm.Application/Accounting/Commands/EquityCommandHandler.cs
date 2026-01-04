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
