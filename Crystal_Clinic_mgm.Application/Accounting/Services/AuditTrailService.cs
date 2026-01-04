using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class AuditTrailService(IMediator mediator) : IAuditTrailService
    {
        public async Task<Result> RecordAuditAsync(CreateAuditTrailDto dto)
        {
            var command = new CreateAuditTrailCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> GetAuditTrailAsync(int auditTrailId)
        {
            var query = new GetAuditTrailQuery { AuditTrailId = auditTrailId };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAuditTrailByEntityAsync(string entityType, int entityId, int page = 1, int pageSize = 20)
        {
            var query = new GetAuditTrailByEntityQuery { EntityType = entityType, EntityId = entityId, Page = page, PageSize = pageSize };
            return await mediator.Send(query);
        }

        public async Task<Result> SearchAuditTrailAsync(AuditTrailFilterRequest filter)
        {
            var query = new SearchAuditTrailQuery { Filter = filter };
            return await mediator.Send(query);
        }
    }
}
