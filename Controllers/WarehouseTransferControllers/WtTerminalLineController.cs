using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WtTerminalLineController : ControllerBase
    {
        private readonly IWtTerminalLineService _wtTerminalLineService;
        private readonly ILocalizationService _localizationService;

        public WtTerminalLineController(IWtTerminalLineService wtTerminalLineService, ILocalizationService localizationService)
        {
            _wtTerminalLineService = wtTerminalLineService; 
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm WtTerminalLine kayıtlarını getirir
        /// </summary>
        /// <returns>WtTerminalLine listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtTerminalLineDto>>>> GetAll() 
        {
            var result = await _wtTerminalLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// ID'ye göre WtTerminalLine kaydını getirir
        /// </summary>
        /// <param name="id">WtTerminalLine ID</param>
        /// <returns>WtTerminalLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtTerminalLineDto?>>> GetById(long id)
        {
            var result = await _wtTerminalLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre WtTerminalLine kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>WtTerminalLine listesi</returns>
        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtTerminalLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _wtTerminalLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }
        

        /// <summary>
        /// Kullanıcı ID'ye göre WtTerminalLine kayıtlarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>WtTerminalLine listesi</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtTerminalLineDto>>>> GetByUserId(long userId)
        {
            var result = await _wtTerminalLineService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// Tarih aralığına göre WtTerminalLine kayıtlarını getirir
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <returns>WtTerminalLine listesi</returns>
        [HttpGet("daterange")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtTerminalLineDto>>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _wtTerminalLineService.GetByDateRangeAsync(startDate, endDate);
            return StatusCode(result.StatusCode, result);
        }
        

        /// <summary>
        /// Yeni WtTerminalLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak WtTerminalLine bilgileri</param>
        /// <returns>Oluşturulan WtTerminalLine</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtTerminalLineDto>>> Create([FromBody] CreateWtTerminalLineDto createDto)        
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtTerminalLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtTerminalLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek WtTerminalLine ID</param>
        /// <param name="updateDto">Güncellenecek WtTerminalLine bilgileri</param>
        /// <returns>Güncellenmiş WtTerminalLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtTerminalLineDto>>> Update(long id, [FromBody] UpdateWtTerminalLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtTerminalLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtTerminalLine kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek WtTerminalLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _wtTerminalLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
