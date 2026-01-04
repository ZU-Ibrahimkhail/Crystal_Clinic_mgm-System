using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Microsoft.AspNetCore.Authorization;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class FixedAssetController : BaseController
    {
        private readonly IFixedAssetService _fixedAssetService;

        public FixedAssetController(IFixedAssetService fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFixedAssetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _fixedAssetService.CreateFixedAssetAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? categoryId = null,
            [FromQuery] int? branchId = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _fixedAssetService.GetAllFixedAssetsAsync(categoryId, branchId, isActive, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _fixedAssetService.GetFixedAssetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFixedAssetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != id)
                return BadRequest("ID mismatch");

            var result = await _fixedAssetService.UpdateFixedAssetAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/RecordDepreciation")]
        public async Task<IActionResult> RecordDepreciation(int id, [FromBody] RecordDepreciationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _fixedAssetService.RecordDepreciationAsync(id, request.DepreciationAmount);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _fixedAssetService.DeactivateFixedAssetAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/DepreciationSchedule")]
        public async Task<IActionResult> GetDepreciationSchedule(int id)
        {
            var result = await _fixedAssetService.GetDepreciationScheduleAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Depreciating/All")]
        public async Task<IActionResult> GetAllDepreciatingAssets([FromQuery] int? branchId = null)
        {
            var result = await _fixedAssetService.GetAllDepreciatingAssetsAsync(branchId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }

    public class RecordDepreciationRequest
    {
        public decimal DepreciationAmount { get; set; }
    }
}
