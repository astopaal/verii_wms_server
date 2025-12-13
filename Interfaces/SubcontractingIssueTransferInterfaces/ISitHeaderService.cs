using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitHeaderService
    {
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentNoAsync(string documentNo);
        Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto);
        Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);

        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAssignedOrdersAsync(long userId);
        Task<ApiResponse<SitAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId);
        Task<ApiResponse<SitHeaderDto>> GenerateOrderAsync(GenerateSubcontractingIssueOrderRequestDto request);
        Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request);
        Task<ApiResponse<int>> BulkCreateSubcontractingIssueTransferAsync(BulkCreateSitRequestDto request);
        Task<ApiResponse<SitHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
