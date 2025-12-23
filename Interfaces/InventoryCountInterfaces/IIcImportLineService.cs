using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcImportLineService
    {
        Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetAllAsync();
        Task<ApiResponse<IcImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
        Task<ApiResponse<IcImportLineDto>> CreateAsync(CreateIcImportLineDto createDto);
        Task<ApiResponse<IcImportLineDto>> UpdateAsync(long id, UpdateIcImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}