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
    public class SidebarmenuLineController : ControllerBase
    {
        private readonly ISidebarmenuLineService _sidebarmenuLineService;

        public SidebarmenuLineController(ISidebarmenuLineService sidebarmenuLineService)
        {
            _sidebarmenuLineService = sidebarmenuLineService;
        }

        /// <summary>
        /// Tüm SidebarmenuLine kayıtlarını getirir
        /// </summary>
        /// <returns>SidebarmenuLine listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SidebarmenuLineDto>>> GetAll()
        {
            var result = await _sidebarmenuLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre SidebarmenuLine kaydını getirir
        /// </summary>
        /// <param name="id">SidebarmenuLine ID</param>
        /// <returns>SidebarmenuLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SidebarmenuLineDto>> GetById(long id)
        {
            var result = await _sidebarmenuLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni SidebarmenuLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak SidebarmenuLine bilgileri</param>
        /// <returns>Oluşturulan SidebarmenuLine</returns>
        [HttpPost]
        public async Task<ActionResult<SidebarmenuLineDto>> Create([FromBody] CreateSidebarmenuLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _sidebarmenuLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut SidebarmenuLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek SidebarmenuLine ID</param>
        /// <param name="updateDto">Güncellenecek SidebarmenuLine bilgileri</param>
        /// <returns>Güncellenmiş SidebarmenuLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<SidebarmenuLineDto>> Update(long id, [FromBody] UpdateSidebarmenuLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _sidebarmenuLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// SidebarmenuLine kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek SidebarmenuLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _sidebarmenuLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// HeaderId'ye göre SidebarmenuLine kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">Header ID</param>
        /// <returns>SidebarmenuLine listesi</returns>
        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<IEnumerable<SidebarmenuLineDto>>> GetByHeaderId(int headerId)
        {
            var result = await _sidebarmenuLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Page'e göre SidebarmenuLine kaydını getirir
        /// </summary>
        /// <param name="page">Sayfa adı</param>
        /// <returns>SidebarmenuLine detayı</returns>
        [HttpGet("by-page/{page}")]
        public async Task<ActionResult<SidebarmenuLineDto>> GetByPage(string page)
        {
            var result = await _sidebarmenuLineService.GetByPageAsync(page);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sayfalı SidebarmenuLine kayıtlarını getirir
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="sortBy">Sıralama alanı (Id, HeaderId, Page, Title, CreatedDate)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalı SidebarmenuLine listesi</returns>
        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SidebarmenuLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _sidebarmenuLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
