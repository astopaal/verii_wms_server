using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class IcRouteService : IIcRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public IcRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.IcRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcRouteDto>>(entities);
                return ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IcRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var entities = await _unitOfWork.IcRoutes.FindAsync(x => x.ImportLineId == importLineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcRouteDto>>(entities);
                return ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IcRouteDto>> CreateAsync(CreateIcRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcRoute>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.IcRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IcRouteDto>> UpdateAsync(long id, UpdateIcRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.IcRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.IcImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.IcRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.IcImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.IcRoutes.SoftDelete(id);

                    if (willDeleteImportLine)
                    {
                        await _unitOfWork.IcImportLines.SoftDelete(importLineId);
                        if (!hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.ICHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
