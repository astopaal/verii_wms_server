using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrHeaderController : ControllerBase
    {
        private readonly IGrHeaderService _grHeaderService;
        private readonly ILocalizationService _localizationService;

        public GrHeaderController(IGrHeaderService grHeaderService, ILocalizationService localizationService)
        {
            _grHeaderService = grHeaderService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetAll()
        {
            var result = await _grHeaderService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto?>>> GetById(int id)
        {
            var result = await _grHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Create([FromBody] CreateGrHeaderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grHeaderService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Update(int id, [FromBody] UpdateGrHeaderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grHeaderService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _grHeaderService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(int id)
        {
            var result = await _grHeaderService.CompleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-branch/{branchCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetByBranchCode(string branchCode)
        {
            var result = await _grHeaderService.GetByBranchCodeAsync(branchCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetByCustomerCode(string customerCode)
        {
            var result = await _grHeaderService.GetByCustomerCodeAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var result = await _grHeaderService.GetByDateRangeAsync(startDate, endDate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _grHeaderService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("bulkCreate")]
        public async Task<ActionResult<ApiResponse<long>>> BulkCreate([FromBody] BulkCreateGrRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                var error = ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400);
                return StatusCode(error.StatusCode, error);
            }
            var result = await _grHeaderService.BulkCreateAsync(request);
            return StatusCode(result.StatusCode, result);
        }
        
    }
}
