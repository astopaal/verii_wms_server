using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrLineController : ControllerBase
    {
        private readonly IPrLineService _prLineService;

        public PrLineController(IPrLineService prLineService)
        {
            _prLineService = prLineService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrLineDto>>>> GetAll()
        {
            var result = await _prLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PrLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _prLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrLineDto>>> GetById(long id)
        {
            var result = await _prLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _prLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrLineDto>>> Create([FromBody] CreatePrLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrLineDto>>> Update(long id, [FromBody] UpdatePrLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _prLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
