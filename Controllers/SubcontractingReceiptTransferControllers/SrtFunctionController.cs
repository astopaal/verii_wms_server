using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SrtFunctionController : ControllerBase
    {
        private readonly ISrtFunctionService _service;

        public SrtFunctionController(ISrtFunctionService service)
        {
            _service = service;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<SrtOpenOrderHeaderDto>>>> GetSrtOpenOrderHeader(string customerCode)
        {
            var result = await _service.GetSrtOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<SrtOpenOrderLineDto>>>> GetSrtOpenOrderLine(string siparisNoCsv)
        {
            var result = await _service.GetSrtOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}
