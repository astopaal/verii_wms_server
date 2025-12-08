using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShImportLineService
    {
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync();
        Task<ApiResponse<ShImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto);
        Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

