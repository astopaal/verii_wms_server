using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShHeaderService
    {
        Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAllAsync();
        Task<ApiResponse<ShHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<ShHeaderDto>> CreateAsync(CreateShHeaderDto createDto);
        Task<ApiResponse<ShHeaderDto>> UpdateAsync(long id, UpdateShHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAssignedOrdersAsync(long userId);
        Task<ApiResponse<ShAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId);
        Task<ApiResponse<ShHeaderDto>> GenerateShipmentOrderAsync(GenerateShipmentOrderRequestDto request);
        Task<ApiResponse<int>> BulkCreateShipmentAsync(BulkCreateShRequestDto request);
        Task<ApiResponse<PagedResponse<ShHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<ShHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
