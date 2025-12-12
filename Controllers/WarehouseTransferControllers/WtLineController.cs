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
    public class WtLineController : ControllerBase
    {
        private readonly IWtLineService _wtLineService;
        private readonly ILocalizationService _localizationService;

        public WtLineController(IWtLineService wtLineService, ILocalizationService localizationService)
        {
            _wtLineService = wtLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm WtLine kayıtlarını getirir
        /// </summary>
        /// <returns>WtLine listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtLineDto>>>> GetAll()
        {
            var result = await _wtLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre WtLine kaydını getirir
        /// </summary>
        /// <param name="id">WtLine ID</param>
        /// <returns>WtLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtLineDto?>>> GetById(long id)
        {
            var result = await _wtLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre WtLine kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">Header ID</param>
        /// <returns>WtLine listesi</returns>
        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _wtLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Stok koduna göre WtLine kayıtlarını getirir
        /// </summary>
        /// <param name="stockCode">Stok kodu</param>
        /// <returns>WtLine listesi</returns>
        [HttpGet("stock/{stockCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtLineDto>>>> GetByStockCode(string stockCode)
        {
            var result = await _wtLineService.GetByStockCodeAsync(stockCode);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ERP sipariş numarasına göre WtLine kayıtlarını getirir
        /// </summary>
        /// <param name="erpOrderNo">ERP sipariş numarası</param>
        /// <returns>WtLine listesi</returns>
        [HttpGet("erporder/{erpOrderNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtLineDto>>>> GetByErpOrderNo(string erpOrderNo)
        {
            var result = await _wtLineService.GetByErpOrderNoAsync(erpOrderNo);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni WtLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak WtLine bilgileri</param>
        /// <returns>Oluşturulan WtLine</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtLineDto>>> Create([FromBody] CreateWtLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek WtLine ID</param>
        /// <param name="updateDto">Güncellenecek WtLine bilgileri</param>
        /// <returns>Güncellenmiş WtLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtLineDto>>> Update(long id, [FromBody] UpdateWtLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtLine kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek WtLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _wtLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Miktar aralığına göre WtLine kayıtlarını getirir
        /// </summary>
        /// <param name="minQuantity">Minimum miktar</param>
        /// <param name="maxQuantity">Maksimum miktar</param>
        /// <returns>WtLine listesi</returns>
        [HttpGet("quantity")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtLineDto>>>> GetByQuantityRange(
            [FromQuery] decimal minQuantity, 
            [FromQuery] decimal maxQuantity)
        {
            var result = await _wtLineService.GetByQuantityRangeAsync(minQuantity, maxQuantity);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sayfalı WtLine kayıtlarını getirir
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="sortBy">Sıralama alanı (Id, HeaderId, StockCode, Quantity, CreatedDate)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalı WtLine listesi</returns>
        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _wtLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
