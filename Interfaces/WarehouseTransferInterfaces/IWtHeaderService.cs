using System.Collections.Generic;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtHeaderService
    {
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto);
        Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> CompleteAsync(long id);
        // Inter-warehouse transfer için korelasyon anahtarlarıyla toplu oluşturma
        Task<ApiResponse<int>> BulkCreateInterWarehouseTransferAsync(BulkCreateWtRequestDto request);
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedTransferOrdersAsync(long userId);
        Task<ApiResponse<WtAssignedTransferOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId);
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request);
        Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request);
        Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved);
    }
}
