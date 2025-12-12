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
    public class PtLineController : ControllerBase
    {
        private readonly IPtLineService _service;
        private readonly ILocalizationService _localizationService;

        public PtLineController(IPtLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtLineDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PtLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PtLineDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("stock/{stockCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtLineDto>>>> GetByStockCode(string stockCode)
        {
            var result = await _service.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("erpOrder/{erpOrderNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtLineDto>>>> GetByErpOrderNo(string erpOrderNo)
        {
            var result = await _service.GetByErpOrderNoAsync(erpOrderNo);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("quantity-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtLineDto>>>> GetByQuantityRange([FromQuery] decimal minQuantity, [FromQuery] decimal maxQuantity)
        {
            var result = await _service.GetByQuantityRangeAsync(minQuantity, maxQuantity);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PtLineDto>>> Create([FromBody] CreatePtLineDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PtLineDto>>> Update(long id, [FromBody] UpdatePtLineDto updateDto)
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
