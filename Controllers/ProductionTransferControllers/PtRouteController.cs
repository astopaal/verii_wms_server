using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PtRouteController : ControllerBase
    {
        private readonly IPtRouteService _service;
        private readonly ILocalizationService _localizationService;

        public PtRouteController(IPtRouteService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtRouteDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PtRouteDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("importline/{importLineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtRouteDto>>>> GetByImportLineId(long importLineId)
        {
            var result = await _service.GetByImportLineIdAsync(importLineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("serial/{serialNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PtRouteDto>>>> GetBySerialNo(string serialNo)
        {
            var result = await _service.GetBySerialNoAsync(serialNo);
            return StatusCode(result.StatusCode, result);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse<PtRouteDto>>> Create([FromBody] CreatePtRouteDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PtRouteDto>>> Update(long id, [FromBody] UpdatePtRouteDto updateDto)
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
    }
}
