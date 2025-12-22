using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers.ProductionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrFunctionController : ControllerBase
    {
        private readonly IPrFunctionService _prFunctionService;

        public PrFunctionController(IPrFunctionService prFunctionService)
        {
            _prFunctionService = prFunctionService;
        }

        [HttpGet("getProductionHeader")]
        public async Task<ActionResult<ApiResponse<List<ProductHeaderDto>>>> GetProductHeader([FromQuery] string isemriNo)
        {
            var result = await _prFunctionService.GetProductHeaderAsync(isemriNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getProductionLines")]
        public async Task<ActionResult<ApiResponse<List<ProductLineDto>>>> GetProductLines(
            [FromQuery] string? isemriNo = null,
            [FromQuery] string? fisNo = null,
            [FromQuery] string? mamulKodu = null)
        {
            var result = await _prFunctionService.GetProductLinesAsync(isemriNo, fisNo, mamulKodu);
            return StatusCode(result.StatusCode, result);
        }
    }
}
