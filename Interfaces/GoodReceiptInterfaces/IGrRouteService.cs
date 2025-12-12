using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrRouteService
    {
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrRouteDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByImportLineIdAsync(long importLineId);
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<GrRouteDto>> CreateAsync(CreateGrRouteDto createDto);
        Task<ApiResponse<GrRouteDto>> UpdateAsync(long id, UpdateGrRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
