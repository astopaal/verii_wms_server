using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShHeaderService : IShHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShHeaders.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShHeaderDto>>(entities);
                return ApiResponse<IEnumerable<ShHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<ShHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> CreateAsync(CreateShHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WMS_WEBAPI.Models.ShHeader>(createDto);
                await _unitOfWork.ShHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> UpdateAsync(long id, UpdateShHeaderDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<ShHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                existing.IsDeleted = true;
                _unitOfWork.ShHeaders.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                existing.IsCompleted = true;
                existing.CompletionDate = DateTime.UtcNow;
                _unitOfWork.ShHeaders.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
