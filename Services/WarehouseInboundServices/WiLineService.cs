using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiLineService : IWiLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WiLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WiLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WiLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.WiLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WiLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<WiLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WiLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
                var dto = _mapper.Map<WiLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<WiLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WiLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WiLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WiLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        



        public async Task<ApiResponse<WiLineDto>> CreateAsync(CreateWiLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiLine>(createDto);
                var created = await _unitOfWork.WiLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineDto>(created);
                return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineDto>> UpdateAsync(long id, UpdateWiLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineDto>(entity);
                return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
                }

                var hasActiveLineSerials = await _unitOfWork.WiLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("WiLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.WiImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WiLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WiLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.WiLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.WiImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.WiHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
