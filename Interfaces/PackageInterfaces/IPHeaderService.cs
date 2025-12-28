using System.Collections.Generic;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPHeaderService
    {
        Task<ApiResponse<IEnumerable<PHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PHeaderDto?>> GetByIdAsync(long id);
        Task<ApiResponse<PHeaderDto>> CreateAsync(CreatePHeaderDto createDto);
        Task<ApiResponse<PHeaderDto>> UpdateAsync(long id, UpdatePHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> MatchPlinesWithMatchedStatus(long pHeaderId, bool isMatched);
        Task<ApiResponse<IEnumerable<object>>> GetAvailableHeadersForMappingAsync(string sourceType);
    }
}

