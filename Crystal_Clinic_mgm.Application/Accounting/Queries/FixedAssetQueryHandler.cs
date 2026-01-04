using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Fixed Assets
    public class GetAllFixedAssetsQuery : IRequest<Result>
    {
        public int? CategoryId { get; set; }
        public int? BranchId { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllFixedAssetsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllFixedAssetsQuery, Result>
    {
        public async Task<Result> Handle(GetAllFixedAssetsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.FixedAssets.Where(a => !a.IsDeleted);

                if (request.CategoryId.HasValue)
                    query = query.Where(a => a.CategoryId == request.CategoryId);

                if (request.BranchId.HasValue)
                    query = query.Where(a => a.BranchId == request.BranchId);

                if (request.IsActive.HasValue)
                    query = query.Where(a => a.IsActive == request.IsActive);

                var totalCount = await query.CountAsync(cancellationToken);

                var assets = await query
                    .OrderByDescending(a => a.AcquisitionDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new FixedAssetDto
                    {
                        Id = a.Id,
                        AssetCode = a.AssetCode,
                        Name = a.Name,
                        CategoryId = a.CategoryId,
                        PurchaseValue = a.PurchaseValue,
                        ResidualValue = a.ResidualValue,
                        UsefulLifeMonths = a.UsefulLifeMonths,
                        AcquisitionDate = a.AcquisitionDate,
                        AccumulatedDepreciation = a.AccumulatedDepreciation,
                        CurrentValue = a.PurchaseValue - a.AccumulatedDepreciation,
                        BranchId = a.BranchId,
                        IsActive = a.IsActive,
                        Description = a.Description
                    })
                    .ToListAsync(cancellationToken);

                var result = new { assets, totalCount, pageNumber = request.PageNumber, pageSize = request.PageSize };
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Fixed Assets: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Fixed Asset By Id
    public class GetFixedAssetByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetFixedAssetByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetFixedAssetByIdQuery, Result>
    {
        public async Task<Result> Handle(GetFixedAssetByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = await context.FixedAssets
                    .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

                if (asset == null)
                    return Result.Fail("Fixed Asset not found.");

                var monthsDepreciated = (DateTime.UtcNow - asset.AcquisitionDate).Days / 30;
                var totalDepreciableValue = asset.PurchaseValue - asset.ResidualValue;
                var monthlyDepreciation = totalDepreciableValue / asset.UsefulLifeMonths;

                var dto = new FixedAssetDto
                {
                    Id = asset.Id,
                    AssetCode = asset.AssetCode,
                    Name = asset.Name,
                    CategoryId = asset.CategoryId,
                    PurchaseValue = asset.PurchaseValue,
                    ResidualValue = asset.ResidualValue,
                    UsefulLifeMonths = asset.UsefulLifeMonths,
                    AcquisitionDate = asset.AcquisitionDate,
                    AccumulatedDepreciation = asset.AccumulatedDepreciation,
                    CurrentValue = asset.PurchaseValue - asset.AccumulatedDepreciation,
                    BranchId = asset.BranchId,
                    IsActive = asset.IsActive,
                    Description = asset.Description
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Fixed Asset: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Depreciation Schedule
    public class GetDepreciationScheduleQuery : IRequest<Result>
    {
        public int FixedAssetId { get; set; }
    }

    public class GetDepreciationScheduleQueryHandler(ERP_DbContext context) : IRequestHandler<GetDepreciationScheduleQuery, Result>
    {
        public async Task<Result> Handle(GetDepreciationScheduleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var asset = await context.FixedAssets
                    .FirstOrDefaultAsync(a => a.Id == request.FixedAssetId && !a.IsDeleted, cancellationToken);

                if (asset == null)
                    return Result.Fail("Fixed Asset not found.");

                var depreciableValue = asset.PurchaseValue - asset.ResidualValue;
                var monthlyDepreciation = depreciableValue / asset.UsefulLifeMonths;
                var monthsDepreciated = (int)((DateTime.UtcNow - asset.AcquisitionDate).TotalDays / 30);
                var netBookValue = asset.PurchaseValue - asset.AccumulatedDepreciation;

                var schedule = new DepreciationScheduleDto
                {
                    FixedAssetId = asset.Id,
                    AssetCode = asset.AssetCode,
                    AssetName = asset.Name,
                    AcquisitionDate = asset.AcquisitionDate,
                    PurchaseValue = asset.PurchaseValue,
                    AccumulatedDepreciation = asset.AccumulatedDepreciation,
                    NetBookValue = netBookValue,
                    ResidualValue = asset.ResidualValue
                };

                return Result.Success(schedule);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Depreciation Schedule: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get All Depreciating Assets
    public class GetAllDepreciatingAssetsQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
    }

    public class GetAllDepreciatingAssetsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllDepreciatingAssetsQuery, Result>
    {
        public async Task<Result> Handle(GetAllDepreciatingAssetsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.FixedAssets.Where(a => !a.IsDeleted && a.IsActive);

                if (request.BranchId.HasValue)
                    query = query.Where(a => a.BranchId == request.BranchId);

                var assets = await query
                    .Select(a => new FixedAssetDepreciationDto
                    {
                        Id = a.Id,
                        AssetCode = a.AssetCode,
                        AcquisitionDate = a.AcquisitionDate,
                        MonthlyDepreciation = (a.PurchaseValue - a.ResidualValue) / a.UsefulLifeMonths,
                        AccumulatedDepreciation = a.AccumulatedDepreciation,
                        CurrentNetValue = a.PurchaseValue - a.AccumulatedDepreciation
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(assets);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Depreciating Assets: {ex.Message}");
            }
        }
    }
    #endregion
}
