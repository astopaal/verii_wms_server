using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShLineService
    {
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<ShLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<ShLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<ShLineDto>> CreateAsync(CreateShLineDto createDto);
        Task<ApiResponse<ShLineDto>> UpdateAsync(long id, UpdateShLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
