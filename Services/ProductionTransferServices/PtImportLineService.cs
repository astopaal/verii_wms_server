using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PtImportLineService : IPtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PtImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineNotFound"), _localizationService.GetLocalizedString("PtImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<PtImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<PtImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PtImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PtImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.PtImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.PtImportLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        public async Task<ApiResponse<PtImportLineDto>> CreateAsync(CreatePtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PtImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtImportLineDto>(entity);
                return ApiResponse<PtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtImportLineDto>> UpdateAsync(long id, UpdatePtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineNotFound"), _localizationService.GetLocalizedString("PtImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtImportLineDto>(entity);
                return ApiResponse<PtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PtImportLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineNotFound"), _localizationService.GetLocalizedString("PtImportLineNotFound"), 404);
                }
                await _unitOfWork.PtImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
