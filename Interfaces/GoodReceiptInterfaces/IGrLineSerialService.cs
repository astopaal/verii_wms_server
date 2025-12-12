using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrLineSerialService
    {
        Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<GrLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetByImportLineIdAsync(long importLineId);
        Task<ApiResponse<GrLineSerialDto>> CreateAsync(CreateGrLineSerialDto createDto);
        Task<ApiResponse<GrLineSerialDto>> UpdateAsync(long id, UpdateGrLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
