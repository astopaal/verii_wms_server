using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtHeaderService
    {
        Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PtHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetByBranchCodeAsync(string branchCode);
        Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<PtHeaderDto>> CreateAsync(CreatePtHeaderDto createDto);
        Task<ApiResponse<PtHeaderDto>> UpdateAsync(long id, UpdatePtHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request);
        Task<ApiResponse<PtHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
