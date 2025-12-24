using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoParameterService
    {
        Task<ApiResponse<IEnumerable<WoParameterDto>>> GetAllAsync();
        Task<ApiResponse<WoParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<WoParameterDto>> CreateAsync(CreateWoParameterDto createDto);
        Task<ApiResponse<WoParameterDto>> UpdateAsync(long id, UpdateWoParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

