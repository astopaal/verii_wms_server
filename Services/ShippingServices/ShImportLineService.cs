using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShImportLineService : IShImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);
                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShImportLineDto>(entity);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);
                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);
                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.StockCode == stockCode);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);
                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShImportLine>(createDto);
                var created = await _unitOfWork.ShImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(created);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(entity);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.ShImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

