using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShTerminalLineService : IShTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShTerminalLines.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShTerminalLineNotFound"); return ApiResponse<ShTerminalLineDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShTerminalLineDto>(entity);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.FindAsync(x => x.TerminalUserId == userId);
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<ShTerminalLineDto>> CreateAsync(CreateShTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShTerminalLine>(createDto);
                var created = await _unitOfWork.ShTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShTerminalLineDto>(created);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShTerminalLineDto>> UpdateAsync(long id, UpdateShTerminalLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShTerminalLines.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShTerminalLineNotFound"); return ApiResponse<ShTerminalLineDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShTerminalLineDto>(entity);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.ShTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
