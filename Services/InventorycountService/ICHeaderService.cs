using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class IcHeaderService : IIcHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public IcHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ICHeaders.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcHeaderDto>>(entities);
                return ApiResponse<IEnumerable<IcHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ICHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.ICHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.ICHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.ICHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.ICHeaders.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                await _unitOfWork.ICHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }
    }
}
