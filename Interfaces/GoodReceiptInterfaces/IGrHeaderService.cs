using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using System;
using System.Collections.Generic;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrHeaderService
    {
        Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrHeaderDto?>> GetByIdAsync(int id);
        Task<ApiResponse<GrHeaderDto>> CreateAsync(CreateGrHeaderDto createDto);
        Task<ApiResponse<GrHeaderDto>> UpdateAsync(int id, UpdateGrHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
        Task<ApiResponse<bool>> CompleteAsync(int id);
        Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByCustomerCodeAsync(string customerCode);
        Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ApiResponse<long>> BulkCreateAsync(BulkCreateGrRequestDto request);

        // Yeni eklenenler:
        Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByBranchCodeAsync(string branchCode);
    }
}
