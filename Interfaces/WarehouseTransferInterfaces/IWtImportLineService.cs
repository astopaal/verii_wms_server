using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtImportLineService
    {
        Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetAllAsync();
        Task<ApiResponse<WtImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByRouteIdAsync(long routeId);
        Task<ApiResponse<WtImportLineDto>> CreateAsync(CreateWtImportLineDto createDto);
        Task<ApiResponse<WtImportLineDto>> UpdateAsync(long id, UpdateWtImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<WtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWtImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}
