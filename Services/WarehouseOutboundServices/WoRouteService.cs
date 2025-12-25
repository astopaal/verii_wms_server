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

                // Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
                var remainingRoutesCount = await _unitOfWork.WoRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);

                // Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
                var shouldDeleteImportLine = remainingRoutesCount == 0;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // Route'u sil
                    await _unitOfWork.WoRoutes.SoftDelete(id);

                    // Eğer bu son route ise, ImportLine'ı da sil
                    if (shouldDeleteImportLine)
                    {
                        var importLine = await _unitOfWork.WoImportLines.GetByIdAsync(importLineId);
                        if (importLine != null && !importLine.IsDeleted)
                        {
                            await _unitOfWork.WoImportLines.SoftDelete(importLineId);
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
