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
                var query = _unitOfWork.SrtRoutes.AsQueryable().Where(r => r.ImportLine.StockCode == stockCode && !r.IsDeleted);
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
                var entities = await _unitOfWork.SrtRoutes.FindAsync(x => x.SerialNo == serialNo && !x.IsDeleted);
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
                await _unitOfWork.SrtRoutes.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtRouteDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
