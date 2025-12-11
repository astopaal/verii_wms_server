using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtHeaderService
    {
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByBranchCodeAsync(string branchCode);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentNoAsync(string documentNo);
        Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto);
        Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);

        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAssignedOrdersAsync(long userId);
        Task<ApiResponse<SrtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId);
        Task<ApiResponse<SrtHeaderDto>> GenerateOrderAsync(GenerateSubcontractingReceiptOrderRequestDto request);
        Task<ApiResponse<int>> BulkCreateSubcontractingReceiptTransferAsync(BulkCreateSrtRequestDto request);
        Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<SrtHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
