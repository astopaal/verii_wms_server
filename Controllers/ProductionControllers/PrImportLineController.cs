using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrImportLineController : ControllerBase
    {
        private readonly IPrImportLineService _prImportLineService;

        public PrImportLineController(IPrImportLineService prImportLineService)
        {
            _prImportLineService = prImportLineService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrImportLineDto>>>> GetAll()
        {
            var result = await _prImportLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrImportLineDto>>> GetById(long id)
        {
            var result = await _prImportLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrImportLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _prImportLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrImportLineDto>>>> GetByLineId(long lineId)
        {
            var result = await _prImportLineService.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrImportLineDto>>> Create([FromBody] CreatePrImportLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prImportLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrImportLineDto>>> Update(long id, [FromBody] UpdatePrImportLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prImportLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _prImportLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
