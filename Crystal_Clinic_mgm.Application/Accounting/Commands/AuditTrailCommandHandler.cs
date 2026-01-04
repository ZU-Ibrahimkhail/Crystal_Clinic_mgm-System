using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Audit Trail
    public class CreateAuditTrailCommand : IRequest<Result>
    {
        public CreateAuditTrailDto Dto { get; set; } = null!;
    }

    public class CreateAuditTrailCommandHandler(ERP_DbContext context) : IRequestHandler<CreateAuditTrailCommand, Result>
    {
        public async Task<Result> Handle(CreateAuditTrailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var auditTrail = new AuditTrail
                {
                    EntityType = request.Dto.EntityType,
                    EntityId = request.Dto.EntityId,
                    Action = request.Dto.Action,
                    AuditDate = DateTime.UtcNow,
                    UserId = request.Dto.UserId,
                    UserName = request.Dto.UserName,
                    BeforeValues = request.Dto.BeforeValues,
                    AfterValues = request.Dto.AfterValues,
                    CorrelationId = request.Dto.CorrelationId ?? Guid.NewGuid().ToString(),
                    IpAddress = request.Dto.IpAddress,
                    UserAgent = request.Dto.UserAgent,
                    RelatedEntityId = request.Dto.RelatedEntityId,
                    RelatedEntityType = request.Dto.RelatedEntityType
                };

                context.AuditTrails.Add(auditTrail);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = auditTrail.Id, correlationId = auditTrail.CorrelationId });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating audit trail: {ex.Message}");
            }
        }
    }
    #endregion
}
