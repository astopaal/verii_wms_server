using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtTerminalLineService : ISrtTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SrtTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineNotFound"), _localizationService.GetLocalizedString("SrtTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<SrtTerminalLineDto>> CreateAsync(CreateSrtTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtTerminalLine>(createDto);
                await _unitOfWork.SrtTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtTerminalLineDto>> UpdateAsync(long id, UpdateSrtTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineNotFound"), _localizationService.GetLocalizedString("SrtTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.SrtTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
