using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShFunctionController : ControllerBase
    {
        private readonly IShFunctionService _service;

        public ShFunctionController(IShFunctionService service)
        {
            _service = service;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<TransferOpenOrderHeaderDto>>>> GetShippingOpenOrderHeader(string customerCode)
        {
            var result = await _service.GetShippingOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<TransferOpenOrderLineDto>>>> GetShippingOpenOrderLine(string siparisNoCsv)
        {
            var result = await _service.GetShippingOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}
