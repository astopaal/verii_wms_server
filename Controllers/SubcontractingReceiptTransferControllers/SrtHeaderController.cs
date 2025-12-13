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
    public class SrtHeaderController : ControllerBase
    {
        private readonly ISrtHeaderService _service;
        private readonly ILocalizationService _localizationService;

        public SrtHeaderController(ISrtHeaderService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtHeaderDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SrtHeaderDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        


        [HttpGet("customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtHeaderDto>>>> GetByCustomerCode(string customerCode)
        {
            var result = await _service.GetByCustomerCodeAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("doctype/{documentType}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtHeaderDto>>>> GetByDocumentType(string documentType)
        {
            var result = await _service.GetByDocumentTypeAsync(documentType);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("docno/{documentNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtHeaderDto>>>> GetByDocumentNo(string documentNo)
        {
            var result = await _service.GetByDocumentNoAsync(documentNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> Create([FromBody] CreateSrtHeaderDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> Update(long id, [FromBody] UpdateSrtHeaderDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
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
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtHeaderDto>>>> GetAssignedOrders(long userId)
        {
            var result = await _service.GetAssignedOrdersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAssignedOrderLines/{headerId}")]
        public async Task<ActionResult<ApiResponse<SrtAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId)
        {
            var result = await _service.GetAssignedOrderLinesAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> Generate([FromBody] GenerateSubcontractingReceiptOrderRequestDto request)
        {
            var result = await _service.GenerateOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk-create")]
        public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkCreateSrtRequestDto request)
        {
            var result = await _service.BulkCreateSubcontractingReceiptTransferAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SrtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request)
        {
            var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> SetApproval(long id, [FromQuery] bool approved)
        {
            var result = await _service.SetApprovalAsync(id, approved);
            return StatusCode(result.StatusCode, result);
        }
    }
}
