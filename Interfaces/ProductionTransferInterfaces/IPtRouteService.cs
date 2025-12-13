using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtRouteService
    {
        Task<ApiResponse<IEnumerable<PtRouteDto>>> GetAllAsync();
        Task<ApiResponse<PtRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtRouteDto>>> GetByImportLineIdAsync(long importLineId);
        Task<ApiResponse<IEnumerable<PtRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<PtRouteDto>> CreateAsync(CreatePtRouteDto createDto);
        Task<ApiResponse<PtRouteDto>> UpdateAsync(long id, UpdatePtRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
