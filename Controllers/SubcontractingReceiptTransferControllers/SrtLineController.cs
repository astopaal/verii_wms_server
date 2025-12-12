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
    public class SrtLineController : ControllerBase
    {
        private readonly ISrtLineService _service;
        private readonly ILocalizationService _localizationService;

        public SrtLineController(ISrtLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtLineDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SrtLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SrtLineDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("stock/{stockCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtLineDto>>>> GetByStockCode(string stockCode)
        {
            var result = await _service.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("erpOrder/{erpOrderNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtLineDto>>>> GetByErpOrderNo(string erpOrderNo)
        {
            var result = await _service.GetByErpOrderNoAsync(erpOrderNo);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("quantity-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtLineDto>>>> GetByQuantityRange([FromQuery] decimal minQuantity, [FromQuery] decimal maxQuantity)
        {
            var result = await _service.GetByQuantityRangeAsync(minQuantity, maxQuantity);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SrtLineDto>>> Create([FromBody] CreateSrtLineDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SrtLineDto>>> Update(long id, [FromBody] UpdateSrtLineDto updateDto)
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
    }
}
