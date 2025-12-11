using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoImportLineService
    {
        Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetAllAsync();
        Task<ApiResponse<WoImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<WoImportLineDto>> CreateAsync(CreateWoImportLineDto createDto);
        Task<ApiResponse<WoImportLineDto>> UpdateAsync(long id, UpdateWoImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<WoImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWoImportBarcodeRequestDto request);
    }
}
