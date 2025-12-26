using System.Collections.Generic;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPPackageService
    {
        Task<ApiResponse<IEnumerable<PPackageDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PPackageDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PPackageDto?>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PPackageDto>>> GetByPackingHeaderIdAsync(long packingHeaderId);
        Task<ApiResponse<PPackageDto>> CreateAsync(CreatePPackageDto createDto);
        Task<ApiResponse<PPackageDto>> UpdateAsync(long id, UpdatePPackageDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

