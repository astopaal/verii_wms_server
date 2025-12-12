using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrLineService
    {
        Task<ApiResponse<IEnumerable<GrLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<GrLineDto>> CreateAsync(CreateGrLineDto createDto);
        Task<ApiResponse<GrLineDto>> UpdateAsync(long id, UpdateGrLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
