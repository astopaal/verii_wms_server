using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrHeaderService
    {
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<PrHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByBranchCodeAsync(string branchCode);
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByDocumentTypeAsync(string documentType);
        Task<ApiResponse<PrHeaderDto>> CreateAsync(CreatePrHeaderDto createDto);
        Task<ApiResponse<PrHeaderDto>> UpdateAsync(long id, UpdatePrHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
    }
}
