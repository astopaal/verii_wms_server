using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoTerminalLineService
    {
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<WoTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<WoTerminalLineDto>> CreateAsync(CreateWoTerminalLineDto createDto);
        Task<ApiResponse<WoTerminalLineDto>> UpdateAsync(long id, UpdateWoTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
