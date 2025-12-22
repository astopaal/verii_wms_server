using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers.ProductionTransferControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PtFunctionController : ControllerBase
    {
        private readonly IPtFunctionService _ptFunctionService;

        public PtFunctionController(IPtFunctionService ptFunctionService)
        {
            _ptFunctionService = ptFunctionService;
        }

        [HttpGet("getProductionTransferHeader")]
        public async Task<ActionResult<ApiResponse<List<ProductHeaderDto>>>> GetProductHeader([FromQuery] string isemriNo)
        {
            var result = await _ptFunctionService.GetProductHeaderAsync(isemriNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getProductionTransferLines")]
        public async Task<ActionResult<ApiResponse<List<ProductLineDto>>>> GetProductLines(
            [FromQuery] string? isemriNo = null,
            [FromQuery] string? fisNo = null,
            [FromQuery] string? mamulKodu = null)
        {
            var result = await _ptFunctionService.GetProductLinesAsync(isemriNo, fisNo, mamulKodu);
            return StatusCode(result.StatusCode, result);
        }
    }
}
