using WMS_WEBAPI.DTOs;

using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrLineService
    {
        Task<ApiResponse<IEnumerable<PrLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PrLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PrLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PrLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<PrLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<PrLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<PrLineDto>> CreateAsync(CreatePrLineDto createDto);
        Task<ApiResponse<PrLineDto>> UpdateAsync(long id, UpdatePrLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

