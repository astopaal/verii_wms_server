using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtImportLineService : ISrtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SrtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtImportLine>(createDto);
                await _unitOfWork.SrtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.SrtImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
