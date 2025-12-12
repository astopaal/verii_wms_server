using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitLineService : ISitLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SitLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<SitLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.SitLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<SitLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<SitLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SitLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
                }
                var dto = _mapper.Map<SitLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<SitLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SitLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SitLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SitLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.SitLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.SitLines.FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
                return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineDto>> CreateAsync(CreateSitLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineDto>(entity);
                return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineDto>> UpdateAsync(long id, UpdateSitLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineDto>(entity);
                return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
                }
                await _unitOfWork.SitLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
