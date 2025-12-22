using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrTerminalLineService : IGrTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public GrTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.GrTerminalLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.GrTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<GrTerminalLineDto>(entity);
                return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.GrTerminalLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.GrTerminalLines.FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrTerminalLineDto>> CreateAsync(CreateGrTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<GrTerminalLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.GrTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<GrTerminalLineDto>(entity);
                return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrTerminalLineDto>> UpdateAsync(long id, UpdateGrTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.GrTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.GrTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<GrTerminalLineDto>(entity);
                return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.GrTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
                }
                await _unitOfWork.GrTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

