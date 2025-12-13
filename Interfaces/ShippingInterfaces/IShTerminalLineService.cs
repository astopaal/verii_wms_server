using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShTerminalLineService
    {
        Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<ShTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<ShTerminalLineDto>> CreateAsync(CreateShTerminalLineDto createDto);
        Task<ApiResponse<ShTerminalLineDto>> UpdateAsync(long id, UpdateShTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
