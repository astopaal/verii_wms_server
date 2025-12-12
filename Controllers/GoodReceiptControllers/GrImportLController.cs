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
    public class GrImportLController : ControllerBase
    {
        private readonly IGrImportLService _grImportLService;
        private readonly ILocalizationService _localizationService;

        public GrImportLController(IGrImportLService grImportLService, ILocalizationService localizationService)
        {
            _grImportLService = grImportLService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm GrImportL kayıtlarını getirir
        /// </summary>
        /// <returns>GrImportL listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLDto>>>> GetAll()
        {
            var result = await _grImportLService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre GrImportL kaydını getirir
        /// </summary>
        /// <param name="id">GrImportL ID</param>
        /// <returns>GrImportL detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLDto?>>> GetById(long id)
        {
            var result = await _grImportLService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre GrImportL kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">Header ID</param>
        /// <returns>GrImportL listesi</returns>
        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _grImportLService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header-with-routes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLWithRoutesDto>>>> GetWithRoutesByHeaderId(long headerId)
        {
            var result = await _grImportLService.GetWithRoutesByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre GrImportL kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>GrImportL listesi</returns>
        [HttpGet("by-line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLDto>>>> GetByLineId(long lineId)
        {
            var result = await _grImportLService.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Stok koduna göre GrImportL kayıtlarını getirir
        /// </summary>
        /// <param name="stockCode">Stok kodu</param>
        /// <returns>GrImportL listesi</returns>
        [HttpGet("by-stock-code/{stockCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLDto>>>> GetByStockCode(string stockCode)
        {
            var result = await _grImportLService.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Yeni GrImportL kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak GrImportL bilgileri</param>
        /// <returns>Oluşturulan GrImportL</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrImportLDto>>> Create([FromBody] CreateGrImportLDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportLDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }

            var result = await _grImportLService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut GrImportL kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek GrImportL ID</param>
        /// <param name="updateDto">Güncellenecek GrImportL bilgileri</param>
        /// <returns>Güncellenmiş GrImportL</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLDto>>> Update(long id, [FromBody] UpdateGrImportLDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportLDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    ModelState?.ToString() ?? string.Empty,
                    400
                ));
            }

            var result = await _grImportLService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GrImportL kaydını soft delete yapar
        /// </summary>
        /// <param name="id">Silinecek GrImportL ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _grImportLService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrImportLDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _grImportLService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
