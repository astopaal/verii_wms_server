using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiParameterService
    {
        Task<ApiResponse<IEnumerable<WiParameterDto>>> GetAllAsync();
        Task<ApiResponse<WiParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<WiParameterDto>> CreateAsync(CreateWiParameterDto createDto);
        Task<ApiResponse<WiParameterDto>> UpdateAsync(long id, UpdateWiParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

