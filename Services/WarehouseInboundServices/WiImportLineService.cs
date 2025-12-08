using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiImportLineService : IWiImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WiImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiImportLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineNotFound"), _localizationService.GetLocalizedString("WiImportLineNotFound"), 404);
                var dto = _mapper.Map<WiImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<WiImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WiImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.FindAsync(x => x.StockCode == stockCode);
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<WiImportLineDto>> CreateAsync(CreateWiImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiImportLine>(createDto);
                var created = await _unitOfWork.WiImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiImportLineDto>(created);
                return ApiResponse<WiImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiImportLineDto>> UpdateAsync(long id, UpdateWiImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiImportLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineNotFound"), _localizationService.GetLocalizedString("WiImportLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiImportLineDto>(entity);
                return ApiResponse<WiImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.WiImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
