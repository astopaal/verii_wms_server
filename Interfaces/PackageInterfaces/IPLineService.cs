using System.Collections.Generic;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPLineService
    {
        Task<ApiResponse<IEnumerable<PLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PLineDto?>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackageIdAsync(long packageId);
        Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackingHeaderIdAsync(long packingHeaderId);
        Task<ApiResponse<PLineDto>> CreateAsync(CreatePLineDto createDto);
        Task<ApiResponse<PLineDto>> UpdateAsync(long id, UpdatePLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

