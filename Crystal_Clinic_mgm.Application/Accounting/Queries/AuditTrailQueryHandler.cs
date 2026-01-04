using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Audit Trail
    public class GetAuditTrailQuery : IRequest<Result>
    {
        public int AuditTrailId { get; set; }
    }

    public class GetAuditTrailQueryHandler(ERP_DbContext context) : IRequestHandler<GetAuditTrailQuery, Result>
    {
        public async Task<Result> Handle(GetAuditTrailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var auditTrail = await context.AuditTrails
                    .FirstOrDefaultAsync(a => a.Id == request.AuditTrailId && !a.IsDeleted, cancellationToken);

                if (auditTrail == null)
                    return Result.Fail("Audit trail not found");

                var dto = new AuditTrailDto
                {
                    Id = auditTrail.Id,
                    EntityType = auditTrail.EntityType,
                    EntityId = auditTrail.EntityId,
                    Action = auditTrail.Action,
                    AuditDate = auditTrail.AuditDate,
                    UserId = auditTrail.UserId,
                    UserName = auditTrail.UserName,
                    BeforeValues = auditTrail.BeforeValues,
                    AfterValues = auditTrail.AfterValues,
                    CorrelationId = auditTrail.CorrelationId,
                    IpAddress = auditTrail.IpAddress,
                    UserAgent = auditTrail.UserAgent,
                    RelatedEntityId = auditTrail.RelatedEntityId,
                    RelatedEntityType = auditTrail.RelatedEntityType
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving audit trail: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Audit Trail By Entity
    public class GetAuditTrailByEntityQuery : IRequest<Result>
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAuditTrailByEntityQueryHandler(ERP_DbContext context) : IRequestHandler<GetAuditTrailByEntityQuery, Result>
    {
        public async Task<Result> Handle(GetAuditTrailByEntityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var auditTrails = await context.AuditTrails
                    .Where(a => !a.IsDeleted && a.EntityType == request.EntityType && a.EntityId == request.EntityId)
                    .OrderByDescending(a => a.AuditDate)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = auditTrails.Select(a => new AuditTrailDto
                {
                    Id = a.Id,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Action = a.Action,
                    AuditDate = a.AuditDate,
                    UserId = a.UserId,
                    UserName = a.UserName,
                    BeforeValues = a.BeforeValues,
                    AfterValues = a.AfterValues,
                    CorrelationId = a.CorrelationId,
                    IpAddress = a.IpAddress,
                    UserAgent = a.UserAgent,
                    RelatedEntityId = a.RelatedEntityId,
                    RelatedEntityType = a.RelatedEntityType
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Page, pageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving audit trail: {ex.Message}");
            }
        }
    }
    #endregion

    #region Search Audit Trail
    public class SearchAuditTrailQuery : IRequest<Result>
    {
        public AuditTrailFilterRequest Filter { get; set; } = null!;
    }

    public class SearchAuditTrailQueryHandler(ERP_DbContext context) : IRequestHandler<SearchAuditTrailQuery, Result>
    {
        public async Task<Result> Handle(SearchAuditTrailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.AuditTrails
                    .Where(a => !a.IsDeleted);

                if (!string.IsNullOrEmpty(request.Filter.EntityType))
                    query = query.Where(a => a.EntityType == request.Filter.EntityType);

                if (request.Filter.EntityId > 0)
                    query = query.Where(a => a.EntityId == request.Filter.EntityId);

                if (!string.IsNullOrEmpty(request.Filter.Action))
                    query = query.Where(a => a.Action == request.Filter.Action);

                if (request.Filter.UserId.HasValue)
                    query = query.Where(a => a.UserId == request.Filter.UserId);

                if (request.Filter.FromDate.HasValue)
                    query = query.Where(a => a.AuditDate >= request.Filter.FromDate);

                if (request.Filter.ToDate.HasValue)
                    query = query.Where(a => a.AuditDate <= request.Filter.ToDate);

                var auditTrails = await query
                    .OrderByDescending(a => a.AuditDate)
                    .Skip((request.Filter.Page - 1) * request.Filter.PageSize)
                    .Take(request.Filter.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = auditTrails.Select(a => new AuditTrailDto
                {
                    Id = a.Id,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Action = a.Action,
                    AuditDate = a.AuditDate,
                    UserId = a.UserId,
                    UserName = a.UserName,
                    BeforeValues = a.BeforeValues,
                    AfterValues = a.AfterValues,
                    CorrelationId = a.CorrelationId,
                    IpAddress = a.IpAddress,
                    UserAgent = a.UserAgent,
                    RelatedEntityId = a.RelatedEntityId,
                    RelatedEntityType = a.RelatedEntityType
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Filter.Page, pageSize = request.Filter.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error searching audit trail: {ex.Message}");
            }
        }
    }
    #endregion
}
