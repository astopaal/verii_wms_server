using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtParameterService
    {
        Task<ApiResponse<IEnumerable<SrtParameterDto>>> GetAllAsync();
        Task<ApiResponse<SrtParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<SrtParameterDto>> CreateAsync(CreateSrtParameterDto createDto);
        Task<ApiResponse<SrtParameterDto>> UpdateAsync(long id, UpdateSrtParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

