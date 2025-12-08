using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShLineService
    {
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<ShLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<ShLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<ShLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<ShLineDto>> CreateAsync(CreateShLineDto createDto);
        Task<ApiResponse<ShLineDto>> UpdateAsync(long id, UpdateShLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

