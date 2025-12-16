using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrRouteService : IGrRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public GrRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.GrRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
                return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("GrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<GrRouteDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.GrRoutes.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<GrRouteDto>>(items);
                var result = new PagedResponse<GrRouteDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<GrRouteDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("GrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var item = await _unitOfWork.GrRoutes.GetByIdAsync(id);
                if (item == null || item.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
                    return ApiResponse<GrRouteDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<GrRouteDto>(item);
                return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("GrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var items = await _unitOfWork.GrRoutes.FindAsync(x => x.ImportLineId == importLineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
                return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("GrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var query = _unitOfWork.GrRoutes.AsQueryable().Where(x => !x.IsDeleted && x.ImportLine.HeaderId == headerId && x.ImportLine.IsDeleted == false);
                var items = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
                return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("GrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrRouteDto>> CreateAsync(CreateGrRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<GrRoute>(createDto);
                await _unitOfWork.GrRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<GrRouteDto>(entity);
                return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("GrRouteCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrRouteDto>> UpdateAsync(long id, UpdateGrRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.GrRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
                    return ApiResponse<GrRouteDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.GrRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<GrRouteDto>(entity);
                return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("GrRouteUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.GrRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.GrImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.GrRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.GrImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.GrLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.GrLineSerials
                        .AsQueryable()
                        .AnyAsync(ls => !ls.IsDeleted && ls.ImportLineId == importLine.Id);
                    if (hasActiveLineSerials)
                    {
                        canDeleteImportLine = false;
                    }
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.GrRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.GrImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.GrHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrRouteDeleteError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
