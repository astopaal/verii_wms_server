using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtRouteService : ISrtRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SrtRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
                return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtRouteNotFound");
                    return ApiResponse<SrtRouteDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtRouteDto>(entity);
                return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var query = _unitOfWork.SrtRoutes.AsQueryable().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()) && !r.IsDeleted);
                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
                return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var sn = (serialNo ?? "").Trim();
                var entities = await _unitOfWork.SrtRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
                return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.SrtRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
                return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.SrtRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
                return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<SrtRouteDto>> CreateAsync(CreateSrtRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtRoute>(createDto);
                await _unitOfWork.SrtRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtRouteDto>(entity);
                return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtRouteDto>> UpdateAsync(long id, UpdateSrtRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteNotFound"), _localizationService.GetLocalizedString("SrtRouteNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtRouteDto>(entity);
                return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.SrtRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteNotFound"), _localizationService.GetLocalizedString("SrtRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.SrtImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.SrtRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0L;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.SrtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.SrtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.SrtLineSerials
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
                    await _unitOfWork.SrtRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.SrtImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.SrtHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
