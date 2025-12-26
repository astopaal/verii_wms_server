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
    public class PPackageController : ControllerBase
    {
        private readonly IPPackageService _pPackageService;
        private readonly ILocalizationService _localizationService;

        public PPackageController(IPPackageService pPackageService, ILocalizationService localizationService)
        {
            _pPackageService = pPackageService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PPackageDto>>>> GetAll()
        {
            var result = await _pPackageService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PPackageDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _pPackageService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PPackageDto?>>> GetById(long id)
        {
            var result = await _pPackageService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{packingHeaderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PPackageDto>>>> GetByPackingHeaderId(long packingHeaderId)
        {
            var result = await _pPackageService.GetByPackingHeaderIdAsync(packingHeaderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PPackageDto>>> Create([FromBody] CreatePPackageDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PPackageDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _pPackageService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PPackageDto>>> Update(long id, [FromBody] UpdatePPackageDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PPackageDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _pPackageService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _pPackageService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

