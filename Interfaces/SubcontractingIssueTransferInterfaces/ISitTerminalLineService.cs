using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitTerminalLineService
    {
        Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<SitTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<SitTerminalLineDto>> CreateAsync(CreateSitTerminalLineDto createDto);
        Task<ApiResponse<SitTerminalLineDto>> UpdateAsync(long id, UpdateSitTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
