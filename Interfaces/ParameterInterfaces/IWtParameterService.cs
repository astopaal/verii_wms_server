using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtParameterService
    {
        Task<ApiResponse<IEnumerable<WtParameterDto>>> GetAllAsync();
        Task<ApiResponse<WtParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<WtParameterDto>> CreateAsync(CreateWtParameterDto createDto);
        Task<ApiResponse<WtParameterDto>> UpdateAsync(long id, UpdateWtParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

