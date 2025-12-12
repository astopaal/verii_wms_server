using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitLineService
    {
        Task<ApiResponse<IEnumerable<SitLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SitLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SitLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SitLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<SitLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<SitLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<SitLineDto>> CreateAsync(CreateSitLineDto createDto);
        Task<ApiResponse<SitLineDto>> UpdateAsync(long id, UpdateSitLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
