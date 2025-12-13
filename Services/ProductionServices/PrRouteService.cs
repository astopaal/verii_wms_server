using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrRouteService : IPrRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PrRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
                return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
                }
                var dto = _mapper.Map<PrRouteDto>(entity);
                return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var entities = await _unitOfWork.PrRoutes.FindAsync(x => x.ImportLineId == importLineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
                return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var entities = await _unitOfWork.PrRoutes.FindAsync(x => x.SerialNo == serialNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
                return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.PrRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
                return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.PrRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
                return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("PrRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<PrRouteDto>> CreateAsync(CreatePrRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrRoute>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrRouteDto>(entity);
                return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrRouteDto>> UpdateAsync(long id, UpdatePrRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrRouteDto>(entity);
                return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PrRoutes.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
                }
                await _unitOfWork.PrRoutes.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrRouteDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrRouteDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
