using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiTerminalLineService
    {
        Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<WiTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<WiTerminalLineDto>> CreateAsync(CreateWiTerminalLineDto createDto);
        Task<ApiResponse<WiTerminalLineDto>> UpdateAsync(long id, UpdateWiTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
