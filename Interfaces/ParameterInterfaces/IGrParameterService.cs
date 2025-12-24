using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrParameterService
    {
        Task<ApiResponse<IEnumerable<GrParameterDto>>> GetAllAsync();
        Task<ApiResponse<GrParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<GrParameterDto>> CreateAsync(CreateGrParameterDto createDto);
        Task<ApiResponse<GrParameterDto>> UpdateAsync(long id, UpdateGrParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

