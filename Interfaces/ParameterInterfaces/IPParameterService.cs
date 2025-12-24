using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPParameterService
    {
        Task<ApiResponse<IEnumerable<PParameterDto>>> GetAllAsync();
        Task<ApiResponse<PParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<PParameterDto>> CreateAsync(CreatePParameterDto createDto);
        Task<ApiResponse<PParameterDto>> UpdateAsync(long id, UpdatePParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

