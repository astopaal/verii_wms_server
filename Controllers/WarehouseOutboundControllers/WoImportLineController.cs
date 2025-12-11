using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WoImportLineController : ControllerBase
    {
        private readonly IWoImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public WoImportLineController(IWoImportLineService service, ILocalizationService localizationService)
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

        [HttpGet("header/{headerId}")]
        public async Task<IActionResult> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("stock/{stockCode}")]
        public async Task<IActionResult> GetByStockCode(string stockCode)
        {
            var result = await _service.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWoImportLineDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateWoImportLineDto updateDto)
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

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<IActionResult> AddBarcodeBasedonAssignedOrder([FromBody] AddWoImportBarcodeRequestDto request)
        {
            var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
