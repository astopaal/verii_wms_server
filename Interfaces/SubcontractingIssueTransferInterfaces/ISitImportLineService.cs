using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitImportLineService
    {
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync();
        Task<ApiResponse<SitImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto);
        Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<SitImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSitImportBarcodeRequestDto request);
    }
}
