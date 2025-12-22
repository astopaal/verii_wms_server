using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiRouteService : IWiRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WiRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WiRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WiRoutes.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WiRouteDto>>(entities);
                return ApiResponse<IEnumerable<WiRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WiRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiRoutes.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WiRouteNotFound"), _localizationService.GetLocalizedString("WiRouteNotFound"), 404);
                var dto = _mapper.Map<WiRouteDto>(entity);
                return ApiResponse<WiRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WiRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiRouteDto>> CreateAsync(CreateWiRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiRoute>(createDto);
                var created = await _unitOfWork.WiRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiRouteDto>(created);
                return ApiResponse<WiRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WiRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiRouteDto>> UpdateAsync(long id, UpdateWiRouteDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiRoutes.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WiRouteNotFound"), _localizationService.GetLocalizedString("WiRouteNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiRouteDto>(entity);
                return ApiResponse<WiRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WiRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.WiRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiRouteNotFound"), _localizationService.GetLocalizedString("WiRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.WiImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.WiRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0L;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WiImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WiLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.WiLineSerials
                        .AsQueryable()
                        .AnyAsync(ls => !ls.IsDeleted && ls.LineId == importLine.LineId.Value);
                    if (hasActiveLineSerials)
                    {
                        canDeleteImportLine = false;
                    }
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WiRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.WiImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.WiHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
