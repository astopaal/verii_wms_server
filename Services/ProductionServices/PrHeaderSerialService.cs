using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrHeaderSerialService : IPrHeaderSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrHeaderSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.PrHeaderSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrHeaderSerialDto>>(items);
                return ApiResponse<IEnumerable<PrHeaderSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrHeaderSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaderSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
                }
                var dto = _mapper.Map<PrHeaderSerialDto>(entity);
                return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var items = await _unitOfWork.PrHeaderSerials.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrHeaderSerialDto>>(items);
                return ApiResponse<IEnumerable<PrHeaderSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrHeaderSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderSerialDto>> CreateAsync(CreatePrHeaderSerialDto createDto)
        {
            try
            {
                var headerExists = await _unitOfWork.PrHeaders.ExistsAsync(createDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderNotFound"), _localizationService.GetLocalizedString("PrHeaderNotFound"), 400);
                }
                var entity = _mapper.Map<PrHeaderSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.PrHeaderSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrHeaderSerialDto>(entity);
                return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderSerialDto>> UpdateAsync(long id, UpdatePrHeaderSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaderSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                
                _unitOfWork.PrHeaderSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                
                var dto = _mapper.Map<PrHeaderSerialDto>(entity);
                return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaderSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                     return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
                }

                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.Now;
                
                _unitOfWork.PrHeaderSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
