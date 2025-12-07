using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoHeaderService
    {
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<WoHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByBranchCodeAsync(string branchCode);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDocumentNoAsync(string documentNo);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByOutboundTypeAsync(string outboundType);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByAccountCodeAsync(string accountCode);
        Task<ApiResponse<WoHeaderDto>> CreateAsync(CreateWoHeaderDto createDto);
        Task<ApiResponse<WoHeaderDto>> UpdateAsync(long id, UpdateWoHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAssignedOrdersAsync(long userId);
        Task<ApiResponse<WoAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId);
    }
}
