using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcParameterService
    {
        Task<ApiResponse<IEnumerable<IcParameterDto>>> GetAllAsync();
        Task<ApiResponse<IcParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<IcParameterDto>> CreateAsync(CreateIcParameterDto createDto);
        Task<ApiResponse<IcParameterDto>> UpdateAsync(long id, UpdateIcParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

