using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PtRouteService : IPtRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PtRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PtRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtRouteDto>>(entities);
                return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PtRouteNotFound");
                    return ApiResponse<PtRouteDto>.ErrorResult(notFound, notFound, 404);
                }
                var dto = _mapper.Map<PtRouteDto>(entity);
                return ApiResponse<PtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var entities = await _unitOfWork.PtRoutes.FindAsync(x => x.ImportLineId == importLineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtRouteDto>>(entities);
                return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var sn = (serialNo ?? "").Trim();
                var entities = await _unitOfWork.PtRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtRouteDto>>(entities);
                return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.PtRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtRouteDto>>(entities);
                return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.PtRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtRouteDto>>(entities);
                return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<PtRouteDto>> CreateAsync(CreatePtRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PtRoute>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PtRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtRouteDto>(entity);
                return ApiResponse<PtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PtRouteCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtRouteDto>> UpdateAsync(long id, UpdatePtRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PtRouteNotFound");
                    return ApiResponse<PtRouteDto>.ErrorResult(notFound, notFound, 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PtRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtRouteDto>(entity);
                return ApiResponse<PtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PtRouteUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.PtRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PtRouteNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.PtImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.PtRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0L;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.PtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.PtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.PtLineSerials
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
                    await _unitOfWork.PtRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.PtImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.PtHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtRouteDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
