using AutoMapper;
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

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySourceWarehouseAsync(string sourceWarehouse)
        {
            try
            {
                // Convert string to int for comparison with SourceWarehouse (int?)
                if (int.TryParse(sourceWarehouse, out int warehouseId))
                {
                    var entities = await _unitOfWork.WtRoutes
                        .FindAsync(x => x.SourceWarehouse == warehouseId && !x.IsDeleted);
                    var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                    return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
                }
                else
                {
                    // If conversion fails, return empty result
                    var emptyDtos = new List<WtRouteDto>();
                    return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(emptyDtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetByTargetWarehouseAsync(string targetWarehouse)
        {
            try
            {
                // Convert string to int for comparison with TargetWarehouse (int?)
                if (int.TryParse(targetWarehouse, out int warehouseId))
                {
                    var entities = await _unitOfWork.WtRoutes
                        .FindAsync(x => x.TargetWarehouse == warehouseId && !x.IsDeleted);
                    var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                    return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
                }
                else
                {
                    // If conversion fails, return empty result
                    var emptyDtos = new List<WtRouteDto>();
                    return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(emptyDtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetByWarehouseIdAsync(long warehouseId)
        {
            try
            {
                // WtRoute model doesn't have WarehouseId property, filtering only by IsDeleted
                var entities = await _unitOfWork.WtRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetByDescriptionAsync(string description)
        {
            try
            {
                var entities = await _unitOfWork.WtRoutes
                    .FindAsync(x => x.Description.Contains(description) && !x.IsDeleted);
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
                var exists = await _unitOfWork.WtRoutes.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                await _unitOfWork.WtRoutes.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtRouteDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
