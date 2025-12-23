using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IcImportLineController : ControllerBase
    {
        private readonly IIcImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public IcImportLineController(IIcImportLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<IcImportLineDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<IcImportLineDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<IcImportLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<IcImportLineDto>>> Create([FromBody] CreateIcImportLineDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<IcImportLineDto>>> Update(long id, [FromBody] UpdateIcImportLineDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("inventoryCountOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>>> InventoryCountOrderCollectedBarcodes(long headerId)
        {
            var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }
    }
}