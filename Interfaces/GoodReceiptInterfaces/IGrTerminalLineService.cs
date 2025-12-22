using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrTerminalLineService
    {
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<GrTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<GrTerminalLineDto>> CreateAsync(CreateGrTerminalLineDto createDto);
        Task<ApiResponse<GrTerminalLineDto>> UpdateAsync(long id, UpdateGrTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

