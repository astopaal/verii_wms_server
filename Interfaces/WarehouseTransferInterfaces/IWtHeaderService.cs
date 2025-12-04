using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtHeaderService
    {
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByBranchCodeAsync(string branchCode);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDocumentNoAsync(string documentNo);
        Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto);
        Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        // Inter-warehouse transfer için korelasyon anahtarlarıyla toplu oluşturma
        Task<ApiResponse<int>> BulkCreateInterWarehouseTransferAsync(BulkCreateWtRequestDto request);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedTransferOrdersAsync(long userId);
        Task<ApiResponse<WtAssignedTransferOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetCompletedAwaitingErpApprovalAsync();
        Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request);
    }
}