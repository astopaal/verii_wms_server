using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShHeaderController : ControllerBase
    {
        private readonly IShHeaderService _service;
        private readonly ILocalizationService _localizationService;

        public ShHeaderController(IShHeaderService service, ILocalizationService localizationService)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShHeaderDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateShHeaderDto updateDto)
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

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id)
        {
            var result = await _service.CompleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShHeaderDto>>>> GetAssignedOrders(long userId)
        {
            var result = await _service.GetAssignedOrdersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("assigned-lines/{headerId}")]
        public async Task<ActionResult<ApiResponse<ShAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId)
        {
            var result = await _service.GetAssignedOrderLinesAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<ShHeaderDto>>> Generate([FromBody] GenerateShipmentOrderRequestDto request)
        {
            var result = await _service.GenerateShipmentOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk-create")]
        public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkCreateShRequestDto request)
        {
            var result = await _service.BulkCreateShipmentAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<ShHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request)
        {
            var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<ShHeaderDto>>> SetApproval(long id, [FromQuery] bool approved)
        {
            var result = await _service.SetApprovalAsync(id, approved);
            return StatusCode(result.StatusCode, result);
        }
    }
}
