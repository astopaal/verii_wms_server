using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtLineService : IWtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<WtLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.WtLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<WtLineDto>>(items);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<PagedResponse<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WtLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<WtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                var dto = _mapper.Map<WtLineDto>(entity);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedStock.Success)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                var finalDto = enrichedStock.Data?.FirstOrDefault() ?? dto;

                return ApiResponse<WtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> CreateAsync(CreateWtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtLine>(createDto);
                entity.IsDeleted = false;

                await _unitOfWork.WtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineDto>(entity);
                return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> UpdateAsync(long id, UpdateWtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.WtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineDto>(entity);
                return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                var hasActiveLineSerials = await _unitOfWork.WtLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("WtLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.WtImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WtLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WtLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.WtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.WtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.WtHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

    }
}
