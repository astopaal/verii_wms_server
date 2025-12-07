using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SitFunctionController : ControllerBase
    {
        private readonly ISitFunctionService _service;

        public SitFunctionController(ISitFunctionService service)
        {
            _service = service;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<SitOpenOrderHeaderDto>>>> GetSitOpenOrderHeader(string customerCode)
        {
            var result = await _service.GetSitOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<SitOpenOrderLineDto>>>> GetSitOpenOrderLine(string siparisNoCsv)
        {
            var result = await _service.GetSitOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}
