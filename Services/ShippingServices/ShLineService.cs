using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShLineService : IShLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public ShLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ShLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.ShLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<ShLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<ShLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<ShLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineNotFound");
                    return ApiResponse<ShLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<ShLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<ShLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        


        public async Task<ApiResponse<ShLineDto>> CreateAsync(CreateShLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShLine>(createDto);
                var created = await _unitOfWork.ShLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineDto>(created);
                return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineDto>> UpdateAsync(long id, UpdateShLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShLines.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineNotFound");
                    return ApiResponse<ShLineDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineDto>(entity);
                return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var hasActiveLineSerials = await _unitOfWork.ShLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == id);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("ShLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.ShImportLines.FindAsync(x => x.LineId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("ShLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.ShLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.ShLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.ShImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.ShHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
