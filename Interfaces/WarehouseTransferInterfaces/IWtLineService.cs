using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtLineService
    {
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WtLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<WtLineDto>> CreateAsync(CreateWtLineDto createDto);
        Task<ApiResponse<WtLineDto>> UpdateAsync(long id, UpdateWtLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
