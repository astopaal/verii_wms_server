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
    public class PrHeaderController : ControllerBase
    {
        private readonly IPrHeaderService _prHeaderService;

        public PrHeaderController(IPrHeaderService prHeaderService)
        {
            _prHeaderService = prHeaderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrHeaderDto>>>> GetAll()
        {
            var result = await _prHeaderService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PrHeaderDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _prHeaderService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrHeaderDto>>> GetById(long id)
        {
            var result = await _prHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrHeaderDto>>> Create([FromBody] CreatePrHeaderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prHeaderService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrHeaderDto>>> Update(long id, [FromBody] UpdatePrHeaderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prHeaderService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _prHeaderService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id)
        {
            var result = await _prHeaderService.CompleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<PrHeaderDto>>> GenerateProductionOrder([FromBody] GenerateProductionOrderRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prHeaderService.GenerateProductionOrderAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
