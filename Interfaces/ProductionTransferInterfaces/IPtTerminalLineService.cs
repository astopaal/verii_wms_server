using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtTerminalLineService
    {
        Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<PtTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<PtTerminalLineDto>> CreateAsync(CreatePtTerminalLineDto createDto);
        Task<ApiResponse<PtTerminalLineDto>> UpdateAsync(long id, UpdatePtTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
