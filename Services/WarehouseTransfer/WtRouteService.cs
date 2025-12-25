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

                // Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
                var remainingRoutesCount = await _unitOfWork.WtRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);

                // Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
                var shouldDeleteImportLine = remainingRoutesCount == 0;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // Route'u sil
                    await _unitOfWork.WtRoutes.SoftDelete(id);

                    // Eğer bu son route ise, ImportLine'ı da sil
                    if (shouldDeleteImportLine)
                    {
                        var importLine = await _unitOfWork.WtImportLines.GetByIdAsync(importLineId);
                        if (importLine != null && !importLine.IsDeleted)
                        {
                            await _unitOfWork.WtImportLines.SoftDelete(importLineId);
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
