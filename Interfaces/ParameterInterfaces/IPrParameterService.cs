using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrParameterService
    {
        Task<ApiResponse<IEnumerable<PrParameterDto>>> GetAllAsync();
        Task<ApiResponse<PrParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<PrParameterDto>> CreateAsync(CreatePrParameterDto createDto);
        Task<ApiResponse<PrParameterDto>> UpdateAsync(long id, UpdatePrParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

