using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/AuditTrail")]
    [ApiController]
    public class AuditTrailController : BaseController
    {
        private readonly IAuditTrailService _auditTrailService;

        public AuditTrailController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditTrail(int id)
        {
            var result = await _auditTrailService.GetAuditTrailAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Entity/{entityType}/{entityId}")]
        public async Task<IActionResult> GetAuditTrailByEntity(
            string entityType,
            int entityId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _auditTrailService.GetAuditTrailByEntityAsync(entityType, entityId, page, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchAuditTrail([FromBody] AuditTrailFilterRequest filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _auditTrailService.SearchAuditTrailAsync(filter);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> ExportAuditTrail([FromBody] AuditTrailFilterRequest filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _auditTrailService.SearchAuditTrailAsync(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(new { message = "Audit trail export initiated", status = "pending", data = result.Value });
        }
    }
}
