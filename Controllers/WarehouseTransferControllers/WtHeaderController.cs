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

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtHeaderDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _wtHeaderService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> GetById(long id)
        {
            var result = await _wtHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("branch/{branchCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetByBranchCode(string branchCode)
        {
            var result = await _wtHeaderService.GetByBranchCodeAsync(branchCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _wtHeaderService.GetByDateRangeAsync(startDate, endDate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetByCustomerCode(string customerCode)
        {
            var result = await _wtHeaderService.GetByCustomerCodeAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("doctype/{documentType}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtHeaderDto>>>> GetByDocumentType(string documentType)
        {
            var result = await _wtHeaderService.GetByDocumentTypeAsync(documentType);
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

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id)
        {
            var result = await _wtHeaderService.CompleteAsync(id);
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

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request)
        {
            var result = await _wtHeaderService.GetCompletedAwaitingErpApprovalPagedAsync(request);
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
