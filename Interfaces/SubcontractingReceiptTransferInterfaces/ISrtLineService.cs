using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtLineService
    {
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto);
        Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
