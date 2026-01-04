using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IAuditTrailService
    {
        Task<Result> RecordAuditAsync(CreateAuditTrailDto dto);
        Task<Result> GetAuditTrailAsync(int auditTrailId);
        Task<Result> GetAuditTrailByEntityAsync(string entityType, int entityId, int page = 1, int pageSize = 20);
        Task<Result> SearchAuditTrailAsync(AuditTrailFilterRequest filter);
    }
}
