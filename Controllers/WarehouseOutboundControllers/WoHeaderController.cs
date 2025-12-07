using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WoHeaderController : ControllerBase
    {
        private readonly IWoHeaderService _service;
        private readonly ILocalizationService _localizationService;

        public WoHeaderController(IWoHeaderService service, ILocalizationService localizationService)
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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? sortBy = null, [FromQuery] string? sortDirection = "asc")
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, sortBy, sortDirection);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("branch/{branchCode}")]
        public async Task<IActionResult> GetByBranchCode(string branchCode)
        {
            var result = await _service.GetByBranchCodeAsync(branchCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _service.GetByDateRangeAsync(startDate, endDate);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("customer/{customerCode}")]
        public async Task<IActionResult> GetByCustomerCode(string customerCode)
        {
            var result = await _service.GetByCustomerCodeAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("doctype/{documentType}")]
        public async Task<IActionResult> GetByDocumentType(string documentType)
        {
            var result = await _service.GetByDocumentTypeAsync(documentType);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("docno/{documentNo}")]
        public async Task<IActionResult> GetByDocumentNo(string documentNo)
        {
            var result = await _service.GetByDocumentNoAsync(documentNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("outbound/{outboundType}")]
        public async Task<IActionResult> GetByOutboundType(string outboundType)
        {
            var result = await _service.GetByOutboundTypeAsync(outboundType);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("account/{accountCode}")]
        public async Task<IActionResult> GetByAccountCode(string accountCode)
        {
            var result = await _service.GetByAccountCodeAsync(accountCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWoHeaderDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateWoHeaderDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id)
        {
            var result = await _service.CompleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WoHeaderDto>>>> GetAssignedOrders(long userId)
        {
            var result = await _service.GetAssignedOrdersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("assigned-lines/{headerId}")]
        public async Task<ActionResult<ApiResponse<WoAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId)
        {
            var result = await _service.GetAssignedOrderLinesAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
