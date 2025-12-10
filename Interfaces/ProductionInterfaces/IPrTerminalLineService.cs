using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrTerminalLineService
    {
        Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<PrTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<PrTerminalLineDto>> CreateAsync(CreatePrTerminalLineDto createDto);
        Task<ApiResponse<PrTerminalLineDto>> UpdateAsync(long id, UpdatePrTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

