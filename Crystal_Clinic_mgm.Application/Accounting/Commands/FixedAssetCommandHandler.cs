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
    #region Create Fixed Asset
    public class CreateFixedAssetCommand : IRequest<Result>
    {
        public CreateFixedAssetDto Dto { get; set; } = null!;
    }

    public class CreateFixedAssetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateFixedAssetCommand, Result>
    {
        public async Task<Result> Handle(CreateFixedAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = new FixedAsset
                {
                    AssetCode = request.Dto.AssetCode,
                    Name = request.Dto.Name,
                    CategoryId = request.Dto.CategoryId,
                    PurchaseValue = request.Dto.PurchaseValue,
                    ResidualValue = request.Dto.ResidualValue,
                    UsefulLifeMonths = request.Dto.UsefulLifeMonths,
                    AcquisitionDate = request.Dto.AcquisitionDate,
                    AccumulatedDepreciation = 0,
                    BranchId = request.Dto.BranchId,
                    IsActive = true,
                    Description = request.Dto.Description,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.FixedAssets.Add(asset);
                await context.SaveChangesAsync(cancellationToken);

                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (companyProfile == null)
                    return Result.Fail("Company profile not configured.");

                if (!companyProfile.FixedAssetAccountId.HasValue)
                    return Result.Fail("Fixed Asset account not configured in company profile.");

                var je = new JournalEntry
                {
                    EntryNumber = $"JE-FA-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = request.Dto.AcquisitionDate,
                    Description = $"Fixed Asset acquisition - {request.Dto.Name}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = request.Dto.AssetCode,
                    ReferenceType = "Fixed Asset",
                    BranchId = request.Dto.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.FixedAssetAccountId.Value,
                    Description = $"Acquisition of {request.Dto.Name}",
                    DebitAmount = request.Dto.PurchaseValue,
                    CreditAmount = 0,
                    AmountInBaseCurrency = request.Dto.PurchaseValue,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.BankAccountId,
                    Description = $"Payment for {request.Dto.Name}",
                    DebitAmount = 0,
                    CreditAmount = request.Dto.PurchaseValue,
                    AmountInBaseCurrency = request.Dto.PurchaseValue,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(je);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(asset.Id, "Fixed Asset created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Fixed Asset: {ex.Message}");
            }
        }
    }
    #endregion

    #region Update Fixed Asset
    public class UpdateFixedAssetCommand : IRequest<Result>
    {
        public UpdateFixedAssetDto Dto { get; set; } = null!;
    }

    public class UpdateFixedAssetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateFixedAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdateFixedAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = await context.FixedAssets
                    .FirstOrDefaultAsync(a => a.Id == request.Dto.Id && !a.IsDeleted, cancellationToken);

                if (asset == null)
                    return Result.Fail("Fixed Asset not found.");

                asset.Name = request.Dto.Name;
                asset.CategoryId = request.Dto.CategoryId;
                asset.ResidualValue = request.Dto.ResidualValue;
                asset.UsefulLifeMonths = request.Dto.UsefulLifeMonths;
                asset.IsActive = request.Dto.IsActive;
                asset.Description = request.Dto.Description;
                asset.ModifiedBy = loggedInUser.Id;
                asset.ModifiedOn = DateTime.UtcNow;

                context.FixedAssets.Update(asset);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Fixed Asset updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Fixed Asset: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Depreciation
    public class RecordDepreciationCommand : IRequest<Result>
    {
        public int FixedAssetId { get; set; }
        public decimal DepreciationAmount { get; set; }
    }

    public class RecordDepreciationCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordDepreciationCommand, Result>
    {
        public async Task<Result> Handle(RecordDepreciationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = await context.FixedAssets
                    .FirstOrDefaultAsync(a => a.Id == request.FixedAssetId && !a.IsDeleted, cancellationToken);

                if (asset == null)
                    return Result.Fail("Fixed Asset not found.");

                var newAccumulated = asset.AccumulatedDepreciation + request.DepreciationAmount;
                var maxDepreciation = asset.PurchaseValue - asset.ResidualValue;

                if (newAccumulated > maxDepreciation)
                    return Result.Fail("Depreciation amount exceeds maximum allowable depreciation.");

                asset.AccumulatedDepreciation = newAccumulated;
                asset.ModifiedBy = loggedInUser.Id;
                asset.ModifiedOn = DateTime.UtcNow;

                context.FixedAssets.Update(asset);
                await context.SaveChangesAsync(cancellationToken);

                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (companyProfile == null)
                    return Result.Fail("Company profile not configured.");

                if (!companyProfile.DepreciationExpenseAccountId.HasValue)
                    return Result.Fail("Depreciation Expense account not configured in company profile.");

                if (!companyProfile.AccumulatedDepreciationAccountId.HasValue)
                    return Result.Fail("Accumulated Depreciation account not configured in company profile.");

                var je = new JournalEntry
                {
                    EntryNumber = $"JE-DEPR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = DateTime.UtcNow,
                    Description = $"Depreciation - {asset.Name}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = asset.AssetCode,
                    ReferenceType = "Depreciation",
                    BranchId = asset.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.DepreciationExpenseAccountId.Value,
                    Description = $"Monthly depreciation for {asset.Name}",
                    DebitAmount = request.DepreciationAmount,
                    CreditAmount = 0,
                    AmountInBaseCurrency = request.DepreciationAmount,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.AccumulatedDepreciationAccountId.Value,
                    Description = $"Accumulated depreciation for {asset.Name}",
                    DebitAmount = 0,
                    CreditAmount = request.DepreciationAmount,
                    AmountInBaseCurrency = request.DepreciationAmount,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(je);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Depreciation recorded successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording depreciation: {ex.Message}");
            }
        }
    }
    #endregion

    #region Deactivate Fixed Asset
    public class DeactivateFixedAssetCommand : IRequest<Result>
    {
        public int FixedAssetId { get; set; }
    }

    public class DeactivateFixedAssetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeactivateFixedAssetCommand, Result>
    {
        public async Task<Result> Handle(DeactivateFixedAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = await context.FixedAssets
                    .FirstOrDefaultAsync(a => a.Id == request.FixedAssetId && !a.IsDeleted, cancellationToken);

                if (asset == null)
                    return Result.Fail("Fixed Asset not found.");

                asset.IsActive = false;
                asset.ModifiedBy = loggedInUser.Id;
                asset.ModifiedOn = DateTime.UtcNow;

                context.FixedAssets.Update(asset);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Fixed Asset deactivated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error deactivating Fixed Asset: {ex.Message}");
            }
        }
    }
    #endregion
}
