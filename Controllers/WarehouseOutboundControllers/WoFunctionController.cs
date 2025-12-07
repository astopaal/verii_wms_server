using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WoFunctionController : ControllerBase
    {
        private readonly IWoFunctionService _service;

        public WoFunctionController(IWoFunctionService service)
        {
            _service = service;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<WoOpenOrderHeaderDto>>>> GetWoOpenOrderHeader(string customerCode)
        {
            var result = await _service.GetWoOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<WoOpenOrderLineDto>>>> GetWoOpenOrderLine(string siparisNoCsv)
        {
            var result = await _service.GetWoOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}
