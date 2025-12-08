using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShRouteController : ControllerBase
    {
        private readonly IShRouteService _service;
        private readonly ILocalizationService _localizationService;

        public ShRouteController(IShRouteService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<IActionResult> GetByLineId(long lineId)
        {
            var result = await _service.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("stock/{stockCode}")]
        public async Task<IActionResult> GetByStockCode(string stockCode)
        {
            var result = await _service.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("serial/{serialNo}")]
        public async Task<IActionResult> GetBySerialNo(string serialNo)
        {
            var result = await _service.GetBySerialNoAsync(serialNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("source/{sourceWarehouse}")]
        public async Task<IActionResult> GetBySourceWarehouse(int sourceWarehouse)
        {
            var result = await _service.GetBySourceWarehouseAsync(sourceWarehouse);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("target/{targetWarehouse}")]
        public async Task<IActionResult> GetByTargetWarehouse(int targetWarehouse)
        {
            var result = await _service.GetByTargetWarehouseAsync(targetWarehouse);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("quantity-range")]
        public async Task<IActionResult> GetByQuantityRange([FromQuery] decimal minQuantity, [FromQuery] decimal maxQuantity)
        {
            var result = await _service.GetByQuantityRangeAsync(minQuantity, maxQuantity);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShRouteDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateShRouteDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

