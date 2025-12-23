using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrImportLineService : IGrImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public GrImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<GrImportLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.GrImportLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<GrImportLineDto>>(items);

                var result = new PagedResponse<GrImportLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<GrImportLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync()
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.GetAllAsync();
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportLineDto?>> GetByIdAsync(long id)
        {
            try
            {
                var grImportLine = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (grImportLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<GrImportLineDto?>.ErrorResult(nf, nf, 404);
                }

                var grImportLineDto = _mapper.Map<GrImportLineDto>(grImportLine);

                var enriched = await _erpService.PopulateStockNamesAsync(new[] { grImportLineDto });
                if (!enriched.Success)
                {
                    return ApiResponse<GrImportLineDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? grImportLineDto;
                
                return ApiResponse<GrImportLineDto?>.SuccessResult(finalDto, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto?>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId)
        {
            try
            {
                var items = await _unitOfWork.GrImportLines
                    .AsQueryable()
                    .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                    .Include(x => x.Routes.Where(r => !r.IsDeleted))
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<GrImportLineWithRoutesDto>>(items);

                var enrichedResp = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedResp.Success)
                {
                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.ErrorResult(enrichedResp.Message, enrichedResp.ExceptionMessage, enrichedResp.StatusCode);
            }
                
                dtos = enrichedResp.Data ?? dtos;

                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.FindAsync(x => x.LineId == lineId);
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }


        public async Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto)
        {
            try
            {
                var grImportLine = _mapper.Map<GrImportLine>(createDto);
                grImportLine.CreatedDate = DateTime.UtcNow;
                
                await _unitOfWork.GrImportLines.AddAsync(grImportLine);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLineDto = _mapper.Map<GrImportLineDto>(grImportLine);
                
                return ApiResponse<GrImportLineDto>.SuccessResult(grImportLineDto, _localizationService.GetLocalizedString("GrImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportLineDto>> UpdateAsync(long id, UpdateGrImportLineDto updateDto)
        {
            try
            {
                var existingGrImportLine = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (existingGrImportLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<GrImportLineDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, existingGrImportLine);
                existingGrImportLine.UpdatedDate = DateTime.UtcNow;
                
                _unitOfWork.GrImportLines.Update(existingGrImportLine);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLineDto = _mapper.Map<GrImportLineDto>(existingGrImportLine);
                
                return ApiResponse<GrImportLineDto>.SuccessResult(grImportLineDto, _localizationService.GetLocalizedString("GrImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                var routes = await _unitOfWork.GrRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("GrImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                var hasActiveLineSerials = await _unitOfWork.GrLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.ImportLineId == entity.Id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("GrImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.GrImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.GrLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.GrImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.GrHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineDeletionError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetImportLinesByHeaderIdAsync(long headerId)
        {
            try
            {
                var importLines = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var importLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(importLines);

                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? importLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }
        
    }
}

