using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoLineService
    {
        Task<ApiResponse<IEnumerable<WoLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WoLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WoLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<WoLineDto>> CreateAsync(CreateWoLineDto createDto);
        Task<ApiResponse<WoLineDto>> UpdateAsync(long id, UpdateWoLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
