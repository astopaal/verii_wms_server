using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitRouteService : ISitRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SitRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitRouteDto>>(entities);
                return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SitRouteNotFound"), _localizationService.GetLocalizedString("SitRouteNotFound"), 404);
                }
                var dto = _mapper.Map<SitRouteDto>(entity);
                return ApiResponse<SitRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var query = _unitOfWork.SitRoutes.AsQueryable().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()) && !r.IsDeleted);
                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SitRouteDto>>(entities);
                return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var sn = (serialNo ?? "").Trim();
                var entities = await _unitOfWork.SitRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitRouteDto>>(entities);
                return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.SitRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitRouteDto>>(entities);
                return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.SitRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitRouteDto>>(entities);
                return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<SitRouteDto>> CreateAsync(CreateSitRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitRoute>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitRouteDto>(entity);
                return ApiResponse<SitRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitRouteDto>> UpdateAsync(long id, UpdateSitRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitRouteNotFound");
                    return ApiResponse<SitRouteDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitRouteDto>(entity);
                return ApiResponse<SitRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.SitRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitRouteNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.SitImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.SitRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0L;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.SitImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.SitLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.SitLineSerials
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
                    await _unitOfWork.SitRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.SitImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.SitHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
