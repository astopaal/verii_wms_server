using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrHeaderService
    {
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PrHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<PrHeaderDto>> CreateAsync(CreatePrHeaderDto createDto);
        Task<ApiResponse<PrHeaderDto>> UpdateAsync(long id, UpdatePrHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        Task<ApiResponse<PrHeaderDto>> GenerateProductionOrderAsync(GenerateProductionOrderRequestDto request);
        Task<ApiResponse<PrHeaderDto>> BulkPrGenerateAsync(BulkPrGenerateRequestDto request);
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAssignedProductionOrdersAsync(long userId);
        Task<ApiResponse<PrAssignedProductionOrderLinesDto>> GetAssignedProductionOrderLinesAsync(long headerId);
        Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request);
        Task<ApiResponse<PrHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
