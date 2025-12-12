using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrImportLService
    {
        Task<ApiResponse<IEnumerable<GrImportLDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrImportLDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrImportLDto?>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<GrImportLWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<GrImportLDto>> CreateAsync(CreateGrImportLDto createDto);
        Task<ApiResponse<GrImportLDto>> UpdateAsync(long id, UpdateGrImportLDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
