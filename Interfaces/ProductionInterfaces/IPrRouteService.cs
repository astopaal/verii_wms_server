using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrRouteService
    {
        Task<ApiResponse<IEnumerable<PrRouteDto>>> GetAllAsync();
        Task<ApiResponse<PrRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrRouteDto>>> GetByImportLineIdAsync(long importLineId);
        Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<PrRouteDto>> CreateAsync(CreatePrRouteDto createDto);
        Task<ApiResponse<PrRouteDto>> UpdateAsync(long id, UpdatePrRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
