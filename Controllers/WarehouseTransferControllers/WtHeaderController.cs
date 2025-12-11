using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WtHeaderController : ControllerBase
    {
        private readonly IWtHeaderService _wtHeaderService;

        public WtHeaderController(IWtHeaderService wtHeaderService)
        {
            _wtHeaderService = wtHeaderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetAll()
        {
            var result = await _wtHeaderService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> GetById(long id)
        {
            var result = await _wtHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> Create([FromBody] CreateWtHeaderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtHeaderService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> Update(long id, [FromBody] UpdateWtHeaderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtHeaderService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _wtHeaderService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("inter-warehouse/bulk-create")]
        public async Task<ActionResult<ApiResponse<int>>> BulkCreateInterWarehouse([FromBody] BulkCreateWtRequestDto request)
        {
            var result = await _wtHeaderService.BulkCreateInterWarehouseTransferAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-document/{documentNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetByDocumentNo(string documentNo)
        {
            var result = await _wtHeaderService.GetByDocumentNoAsync(documentNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetAssignedTransferOrders(long userId)
        {
            var result = await _wtHeaderService.GetAssignedTransferOrdersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAssignedTransferOrderLines/{headerId}")]
        public async Task<ActionResult<ApiResponse<WtAssignedTransferOrderLinesDto>>> GetAssignedTransferOrderLines(long headerId)
        {
            var result = await _wtHeaderService.GetAssignedTransferOrderLinesAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null, [FromQuery] string? sortDirection = "asc")
        {
            var result = await _wtHeaderService.GetCompletedAwaitingErpApprovalPagedAsync(pageNumber, pageSize, sortBy, sortDirection);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> GenerateWarehouseTransferOrder([FromBody] GenerateWarehouseTransferOrderRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtHeaderService.GenerateWarehouseTransferOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> SetApproval(long id, [FromQuery] bool approved)
        {
            var result = await _wtHeaderService.SetApprovalAsync(id, approved);
            return StatusCode(result.StatusCode, result);
        }
    }
}
