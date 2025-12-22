using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiImportLineService
    {
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetAllAsync();
        Task<ApiResponse<WiImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<WiImportLineDto>> CreateAsync(CreateWiImportLineDto createDto);
        Task<ApiResponse<WiImportLineDto>> UpdateAsync(long id, UpdateWiImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<WiImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWiImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<WiImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}
