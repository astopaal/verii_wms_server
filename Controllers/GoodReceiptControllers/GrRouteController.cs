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
    public class GrRouteController : ControllerBase
    {
        private readonly IGrRouteService _service;
        private readonly ILocalizationService _localizationService;

        public GrRouteController(IGrRouteService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrRouteDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrRouteDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("import-line/{importLineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrRouteDto>>>> GetByImportLineId(long importLineId)
        {
            var result = await _service.GetByImportLineIdAsync(importLineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrRouteDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrRouteDto>>> Create([FromBody] CreateGrRouteDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrRouteDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrRouteDto>>> Update(long id, [FromBody] UpdateGrRouteDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrRouteDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrRouteDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
