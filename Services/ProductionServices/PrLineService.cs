using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrLineService : IPrLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PrLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<PrLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PrLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<PrLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<PrLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<PrLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PrLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 404);
                }
                var dto = _mapper.Map<PrLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PrLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PrLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PrLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.PrLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.PrLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<PrLineDto>> CreateAsync(CreatePrLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineDto>(entity);
                return ApiResponse<PrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineDto>> UpdateAsync(long id, UpdatePrLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineDto>(entity);
                return ApiResponse<PrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PrLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 404);
                }
                await _unitOfWork.PrLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
