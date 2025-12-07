using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WiFunctionController : ControllerBase
    {
        private readonly IWiFunctionService _service;

        public WiFunctionController(IWiFunctionService service)
        {
            _service = service;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<WiOpenOrderHeaderDto>>>> GetWiOpenOrderHeader(string customerCode)
        {
            var result = await _service.GetWiOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<WiOpenOrderLineDto>>>> GetWiOpenOrderLine(string siparisNoCsv)
        {
            var result = await _service.GetWiOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}
