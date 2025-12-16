using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrImportLineService : IPrImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PrImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<PrImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PrImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<PrImportLineDto>> CreateAsync(CreatePrImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrImportLineDto>(entity);
                return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrImportLineDto>> UpdateAsync(long id, UpdatePrImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrImportLineDto>(entity);
                return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                var routes = await _unitOfWork.PrRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("PrImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                var hasActiveLineSerials = await _unitOfWork.PrLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("PrImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.PrImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.PrLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.PrImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.PrHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
