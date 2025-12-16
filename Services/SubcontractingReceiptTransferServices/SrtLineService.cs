using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtLineService : ISrtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SrtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.SrtLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<SrtLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<SrtLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SrtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtLineNotFound");
                    return ApiResponse<SrtLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<SrtLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SrtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtLine>(createDto);
                await _unitOfWork.SrtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineDto>(entity);
                return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineDto>(entity);
                return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 404);
                }

                var hasActiveLineSerials = await _unitOfWork.SrtLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("SrtLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.SrtImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("SrtLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.SrtLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.SrtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.SrtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.SrtHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
