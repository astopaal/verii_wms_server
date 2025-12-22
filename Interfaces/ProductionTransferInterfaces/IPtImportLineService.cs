using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtImportLineService
    {
        Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PtImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<PtImportLineDto>> CreateAsync(CreatePtImportLineDto createDto);
        Task<ApiResponse<PtImportLineDto>> UpdateAsync(long id, UpdatePtImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<PtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddPtImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<PtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}
