using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShRouteService
    {
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync();
        Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto);
        Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
