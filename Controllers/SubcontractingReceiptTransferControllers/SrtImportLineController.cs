using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SrtImportLineController : ControllerBase
    {
        private readonly ISrtImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public SrtImportLineController(ISrtImportLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByLineId(long lineId)
        {
            var result = await _service.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Create([FromBody] CreateSrtImportLineDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Update(long id, [FromBody] UpdateSrtImportLineDto updateDto)
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

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddSrtImportBarcodeRequestDto request)
        {
            var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
