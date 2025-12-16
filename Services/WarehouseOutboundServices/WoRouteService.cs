using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoRouteService : IWoRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WoRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WoRoutes.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
                return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoRoutes.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("WoRouteNotFound"); return ApiResponse<WoRouteDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<WoRouteDto>(entity);
                return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var query = _unitOfWork.WoRoutes.AsQueryable().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()));
                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
                return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var sn = (serialNo ?? "").Trim();
                var entities = await _unitOfWork.WoRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)));
                var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
                return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.WoRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse);
                var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
                return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.WoRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse);
                var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
                return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WoRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<WoRouteDto>> CreateAsync(CreateWoRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoRoute>(createDto);
                var created = await _unitOfWork.WoRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoRouteDto>(created);
                return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WoRouteCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoRouteDto>> UpdateAsync(long id, UpdateWoRouteDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoRoutes.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("WoRouteNotFound"); return ApiResponse<WoRouteDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoRouteDto>(entity);
                return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WoRouteUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.WoRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoRouteNotFound"), _localizationService.GetLocalizedString("WoRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;
                var importLine = await _unitOfWork.WoImportLines.GetByIdAsync(importLineId);
                var remainingRoutesUnderImport = await _unitOfWork.WoRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);
                var willDeleteImportLine = remainingRoutesUnderImport == 0 && importLine != null && !importLine.IsDeleted;

                long headerIdToCheck = importLine?.HeaderId ?? 0;
                var hasOtherImportLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WoImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerIdToCheck && il.Id != importLineId)
                    : false;
                var hasOtherLinesUnderHeader = headerIdToCheck != 0
                    ? await _unitOfWork.WoLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerIdToCheck)
                    : false;

                var canDeleteImportLine = true;
                if (willDeleteImportLine && importLine != null && importLine.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.WoLineSerials
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
                    await _unitOfWork.WoRoutes.SoftDelete(id);

                    if (willDeleteImportLine && canDeleteImportLine)
                    {
                        await _unitOfWork.WoImportLines.SoftDelete(importLineId);

                        if (!hasOtherLinesUnderHeader && !hasOtherImportLinesUnderHeader && headerIdToCheck != 0)
                        {
                            await _unitOfWork.WoHeaders.SoftDelete(headerIdToCheck);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoRouteDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
