using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitImportLineService : ISitImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SitImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<SitImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SitImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitImportLineDto>(entity);
                return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitImportLineDto>(entity);
                return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitImportLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                await _unitOfWork.SitImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
