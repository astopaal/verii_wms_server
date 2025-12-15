using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtRouteService
    {
        Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync();
        Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto);
        Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
