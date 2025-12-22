using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers.ProductionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrRouteController : ControllerBase
    {
        private readonly IPrRouteService _prRouteService;

        public PrRouteController(IPrRouteService prRouteService)
        {
            _prRouteService = prRouteService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrRouteDto>>>> GetAll()
        {
            var result = await _prRouteService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrRouteDto>>> GetById(long id)
        {
            var result = await _prRouteService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-import-line/{importLineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrRouteDto>>>> GetByImportLineId(long importLineId)
        {
            var result = await _prRouteService.GetByImportLineIdAsync(importLineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-serial/{serialNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrRouteDto>>>> GetBySerialNo(string serialNo)
        {
            var result = await _prRouteService.GetBySerialNoAsync(serialNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrRouteDto>>> Create([FromBody] CreatePrRouteDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prRouteService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrRouteDto>>> Update(long id, [FromBody] UpdatePrRouteDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _prRouteService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _prRouteService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
