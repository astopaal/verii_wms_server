using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiLineService
    {
        Task<ApiResponse<IEnumerable<WiLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WiLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WiLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WiLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<WiLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<WiLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<WiLineDto>> CreateAsync(CreateWiLineDto createDto);
        Task<ApiResponse<WiLineDto>> UpdateAsync(long id, UpdateWiLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
