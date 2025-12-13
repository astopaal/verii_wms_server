using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiHeaderService
    {
        Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WiHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByInboundTypeAsync(string inboundType);
        Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByAccountCodeAsync(string accountCode);
        Task<ApiResponse<WiHeaderDto>> CreateAsync(CreateWiHeaderDto createDto);
        Task<ApiResponse<WiHeaderDto>> UpdateAsync(long id, UpdateWiHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAssignedOrdersAsync(long userId);
        Task<ApiResponse<WiAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId);
        Task<ApiResponse<WiHeaderDto>> GenerateWarehouseInboundOrderAsync(GenerateWarehouseInboundOrderRequestDto request);
        Task<ApiResponse<int>> BulkCreateWarehouseInboundAsync(BulkCreateWiRequestDto request);
        Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request);
        Task<ApiResponse<WiHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
