using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtRouteService : IWtRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WtRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtRoutes.GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var entities = await _unitOfWork.WtRoutes.FindAsync(x => x.SerialNo == serialNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtRoute>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;

                await _unitOfWork.WtRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;

                _unitOfWork.WtRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.WtRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.WtImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.WtRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.WtLineSerials
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
                    await _unitOfWork.WtRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.WtImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.WtHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
   
    }
}
