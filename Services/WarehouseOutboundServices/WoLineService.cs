using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoLineService : IWoLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WoLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WoLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WoLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WoLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.WoLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WoLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<WoLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WoLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WoLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
                var dto = _mapper.Map<WoLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<WoLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WoLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WoLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WoLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        



        public async Task<ApiResponse<WoLineDto>> CreateAsync(CreateWoLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoLine>(createDto);
                var created = await _unitOfWork.WoLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineDto>(created);
                return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineDto>> UpdateAsync(long id, UpdateWoLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineDto>(entity);
                return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
                }

                var hasActiveLineSerials = await _unitOfWork.WoLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("WoLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.WoImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WoLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WoLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.WoLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.WoImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.WoHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
