using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitTerminalLineService : ISitTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SitTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }


        public async Task<ApiResponse<SitTerminalLineDto>> CreateAsync(CreateSitTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitTerminalLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitTerminalLineDto>> UpdateAsync(long id, UpdateSitTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                await _unitOfWork.SitTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
