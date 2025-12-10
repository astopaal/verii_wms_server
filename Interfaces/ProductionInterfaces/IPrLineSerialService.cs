using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrLineSerialService
    {
        Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<PrLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<PrLineSerialDto>> CreateAsync(CreatePrLineSerialDto createDto);
        Task<ApiResponse<PrLineSerialDto>> UpdateAsync(long id, UpdatePrLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

