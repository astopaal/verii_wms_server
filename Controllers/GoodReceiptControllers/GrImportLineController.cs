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
    public class GrImportLineController : ControllerBase
    {
        private readonly IGrImportLineService _grImportLineService;
        private readonly ILocalizationService _localizationService;

        public GrImportLineController(IGrImportLineService grImportLineService, ILocalizationService localizationService)
        {
            _grImportLineService = grImportLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm GrImportLine kayıtlarını getirir
        /// </summary>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineDto>>>> GetAll()
        {
            var result = await _grImportLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre GrImportLine kaydını getirir
        /// </summary>
        /// <param name="id">GrImportLine ID</param>
        /// <returns>GrImportLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLineDto?>>> GetById(long id)
        {
            var result = await _grImportLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre GrImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">Header ID</param>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _grImportLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header-with-routes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>>> GetWithRoutesByHeaderId(long headerId)
        {
            var result = await _grImportLineService.GetWithRoutesByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre GrImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet("by-line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineDto>>>> GetByLineId(long lineId)
        {
            var result = await _grImportLineService.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Yeni GrImportLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak GrImportLine bilgileri</param>
        /// <returns>Oluşturulan GrImportLine</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrImportLineDto>>> Create([FromBody] CreateGrImportLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }

            var result = await _grImportLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut GrImportLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek GrImportLine ID</param>
        /// <param name="updateDto">Güncellenecek GrImportLine bilgileri</param>
        /// <returns>Güncellenmiş GrImportLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLineDto>>> Update(long id, [FromBody] UpdateGrImportLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }

            var result = await _grImportLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GrImportLine kaydını soft delete yapar
        /// </summary>
        /// <param name="id">Silinecek GrImportLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _grImportLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrImportLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _grImportLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}

