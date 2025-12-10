using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrLineSerialService : IPrLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.PrLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.PrLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> CreateAsync(CreatePrLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.PrLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 400);
                }
                var entity = _mapper.Map<PrLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.PrLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> UpdateAsync(long id, UpdatePrLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.PrLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.PrLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PrLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                await _unitOfWork.PrLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

