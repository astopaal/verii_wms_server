using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoImportLineService : IWoImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WoImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoImportLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineNotFound"), _localizationService.GetLocalizedString("WoImportLineNotFound"), 404);
                var dto = _mapper.Map<WoImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<WoImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WoImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.FindAsync(x => x.StockCode == stockCode);
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<WoImportLineDto>> CreateAsync(CreateWoImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoImportLine>(createDto);
                var created = await _unitOfWork.WoImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoImportLineDto>(created);
                return ApiResponse<WoImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoImportLineDto>> UpdateAsync(long id, UpdateWoImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoImportLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineNotFound"), _localizationService.GetLocalizedString("WoImportLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoImportLineDto>(entity);
                return ApiResponse<WoImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.WoImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
