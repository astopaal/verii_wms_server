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
    public class SidebarmenuHeaderController : ControllerBase
    {
        private readonly ISidebarmenuHeaderService _sidebarmenuHeaderService;

        public SidebarmenuHeaderController(ISidebarmenuHeaderService sidebarmenuHeaderService)
        {
            _sidebarmenuHeaderService = sidebarmenuHeaderService;
        }

        /// <summary>
        /// Tüm SidebarmenuHeader kayıtlarını getirir
        /// </summary>
        /// <returns>SidebarmenuHeader listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SidebarmenuHeaderDto>>> GetAll()
        {
            var result = await _sidebarmenuHeaderService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre SidebarmenuHeader kaydını getirir
        /// </summary>
        /// <param name="id">SidebarmenuHeader ID</param>
        /// <returns>SidebarmenuHeader detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SidebarmenuHeaderDto>> GetById(long id)
        {
            var result = await _sidebarmenuHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni SidebarmenuHeader kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak SidebarmenuHeader bilgileri</param>
        /// <returns>Oluşturulan SidebarmenuHeader</returns>
        [HttpPost]
        public async Task<ActionResult<SidebarmenuHeaderDto>> Create([FromBody] CreateSidebarmenuHeaderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _sidebarmenuHeaderService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut SidebarmenuHeader kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek SidebarmenuHeader ID</param>
        /// <param name="updateDto">Güncellenecek SidebarmenuHeader bilgileri</param>
        /// <returns>Güncellenmiş SidebarmenuHeader</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<SidebarmenuHeaderDto>> Update(long id, [FromBody] UpdateSidebarmenuHeaderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _sidebarmenuHeaderService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// SidebarmenuHeader kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek SidebarmenuHeader ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _sidebarmenuHeaderService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// MenuKey'e göre SidebarmenuHeader kaydını getirir
        /// </summary>
        /// <param name="menuKey">Menu anahtarı</param>
        /// <returns>SidebarmenuHeader detayı</returns>
        [HttpGet("by-menu-key/{menuKey}")]
        public async Task<ActionResult<SidebarmenuHeaderDto>> GetByMenuKey(string menuKey)
        {
            var result = await _sidebarmenuHeaderService.GetByMenuKeyAsync(menuKey);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// RoleLevel'a göre SidebarmenuHeader kayıtlarını getirir
        /// </summary>
        /// <param name="roleLevel">Rol seviyesi</param>
        /// <returns>SidebarmenuHeader listesi</returns>
        [HttpGet("by-role-level/{roleLevel}")]
        public async Task<ActionResult<IEnumerable<SidebarmenuHeaderDto>>> GetByRoleLevel(int roleLevel)
        {
            var result = await _sidebarmenuHeaderService.GetByRoleLevelAsync(roleLevel);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcı ID'sine göre SidebarmenuHeader kayıtlarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>SidebarmenuHeader listesi</returns>
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<List<SidebarmenuHeader>>> GetSidebarmenuHeadersByUserId(int userId)
        {
            var result = await _sidebarmenuHeaderService.GetSidebarmenuHeadersByUserId(userId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sayfalı SidebarmenuHeader kayıtlarını getirir
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="sortBy">Sıralama alanı (Id, MenuKey, Title, RoleLevel, CreatedDate)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalı SidebarmenuHeader listesi</returns>
        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SidebarmenuHeaderDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _sidebarmenuHeaderService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
