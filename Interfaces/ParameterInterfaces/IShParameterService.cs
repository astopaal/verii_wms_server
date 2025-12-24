using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShParameterService
    {
        Task<ApiResponse<IEnumerable<ShParameterDto>>> GetAllAsync();
        Task<ApiResponse<ShParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<ShParameterDto>> CreateAsync(CreateShParameterDto createDto);
        Task<ApiResponse<ShParameterDto>> UpdateAsync(long id, UpdateShParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

