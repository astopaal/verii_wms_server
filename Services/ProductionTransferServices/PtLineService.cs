using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PtLineService : IPtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<PtLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PtLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<PtLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<PtLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<PtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }
                var dto = _mapper.Map<PtLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        



        public async Task<ApiResponse<PtLineDto>> CreateAsync(CreatePtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PtLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineDto>(entity);
                return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineDto>> UpdateAsync(long id, UpdatePtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineDto>(entity);
                return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }

                var hasActiveLineSerials = await _unitOfWork.PtLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("PtLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.PtImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("PtLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.PtLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.PtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.PtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.PtHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
