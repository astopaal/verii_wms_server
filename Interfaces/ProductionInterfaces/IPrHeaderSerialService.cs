using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPrHeaderSerialService
    {
        Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetAllAsync();
        Task<ApiResponse<PrHeaderSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<PrHeaderSerialDto>> CreateAsync(CreatePrHeaderSerialDto createDto);
        Task<ApiResponse<PrHeaderSerialDto>> UpdateAsync(long id, UpdatePrHeaderSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
