using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrImportLineService
    {
        Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrImportLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrImportLineDto?>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto);
        Task<ApiResponse<GrImportLineDto>> UpdateAsync(long id, UpdateGrImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<GrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddGrImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}

