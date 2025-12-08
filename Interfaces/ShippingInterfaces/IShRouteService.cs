using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShRouteService
    {
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync();
        Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse);
        Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto);
        Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

