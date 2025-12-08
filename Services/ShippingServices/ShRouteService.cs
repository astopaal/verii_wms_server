using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShRouteService : IShRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShRoutes.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShRouteNotFound"); return ApiResponse<ShRouteDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShRouteDto>(entity);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.ImportLineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var query = _unitOfWork.ShRoutes.AsQueryable().Where(r => r.ImportLine.StockCode == stockCode && !r.IsDeleted);
                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => (x.SerialNo == serialNo || x.SerialNo2 == serialNo) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShRoute>(createDto);
                var created = await _unitOfWork.ShRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShRouteDto>(created);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShRoutes.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShRouteNotFound"); return ApiResponse<ShRouteDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShRouteDto>(entity);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.ShRoutes.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShRouteDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

