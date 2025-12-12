using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GoodReciptFunctionsController : ControllerBase
    {
        private readonly IGoodReciptFunctionsService _goodReciptFunctionsService;
        private readonly ILocalizationService _localizationService;

        public GoodReciptFunctionsController(IGoodReciptFunctionsService goodReciptFunctionsService, ILocalizationService localizationService)
        {
            _goodReciptFunctionsService = goodReciptFunctionsService;
            _localizationService = localizationService;
        }

        [HttpGet("headers/customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersHeaderDto>>>> GetGoodsReceiptHeadersByCustomer(string customerCode)
        {
            var result = await _goodReciptFunctionsService.GetGoodsReceiptHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        /// <returns>Belirtilen siparişlere ait satır listesi</returns>
        [HttpGet("lines/customer/{customerCode}/orders/{siparisNoCsv?}")]
        public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersLineDto>>>> GetGoodsReceiptLines(string customerCode, string? siparisNoCsv = null)
        {
            var result = await _goodReciptFunctionsService.GetGoodsReceiptLineAsync(siparisNoCsv ?? string.Empty, customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/customer/{customerCode}/branch/{branchCode}")]
        public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersLineDto>>>> GetGoodsReceiptLinesByCustomerAndBranch(string customerCode, string branchCode)
        {
            var result = await _goodReciptFunctionsService.GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(branchCode, customerCode);
            return StatusCode(result.StatusCode, result);
        }
    }
}
