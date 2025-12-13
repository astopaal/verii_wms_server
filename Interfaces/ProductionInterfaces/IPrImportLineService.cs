using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrImportLineService
    {
        Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PrImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<PrImportLineDto>> CreateAsync(CreatePrImportLineDto createDto);
        Task<ApiResponse<PrImportLineDto>> UpdateAsync(long id, UpdatePrImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
