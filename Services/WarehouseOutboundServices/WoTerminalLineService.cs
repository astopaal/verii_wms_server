using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoTerminalLineService : IWoTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WoTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WoTerminalLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoTerminalLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineNotFound"), _localizationService.GetLocalizedString("WoTerminalLineNotFound"), 404);
                var dto = _mapper.Map<WoTerminalLineDto>(entity);
                return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WoTerminalLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.WoTerminalLines.FindAsync(x => x.TerminalUserId == userId);
                var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<WoTerminalLineDto>> CreateAsync(CreateWoTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoTerminalLine>(createDto);
                var created = await _unitOfWork.WoTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoTerminalLineDto>(created);
                return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoTerminalLineDto>> UpdateAsync(long id, UpdateWoTerminalLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoTerminalLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineNotFound"), _localizationService.GetLocalizedString("WoTerminalLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoTerminalLineDto>(entity);
                return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.WoTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
