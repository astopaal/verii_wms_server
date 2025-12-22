using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers.ProductionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrHeaderSerialController : ControllerBase
    {
        private readonly IPrHeaderSerialService _service;
        private readonly ILocalizationService _localizationService;

        public PrHeaderSerialController(IPrHeaderSerialService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrHeaderSerialDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrHeaderSerialDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _service.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> Create([FromBody] CreatePrHeaderSerialDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> Update(long id, [FromBody] UpdatePrHeaderSerialDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
