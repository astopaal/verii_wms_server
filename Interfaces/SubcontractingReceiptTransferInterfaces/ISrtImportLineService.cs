using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtImportLineService
    {
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync();
        Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto);
        Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request);
    }
}
