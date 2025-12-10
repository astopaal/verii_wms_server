using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrTerminalLineService : IPrTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PrTerminalLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineNotFound"), _localizationService.GetLocalizedString("PrTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<PrTerminalLineDto>(entity);
                return ApiResponse<PrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PrTerminalLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.PrTerminalLines.FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var entities = await _unitOfWork.PrTerminalLines.FindAsync(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrTerminalLineDto>> CreateAsync(CreatePrTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrTerminalLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrTerminalLineDto>(entity);
                return ApiResponse<PrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrTerminalLineDto>> UpdateAsync(long id, UpdatePrTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineNotFound"), _localizationService.GetLocalizedString("PrTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrTerminalLineDto>(entity);
                return ApiResponse<PrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PrTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineNotFound"), _localizationService.GetLocalizedString("PrTerminalLineNotFound"), 404);
                }
                await _unitOfWork.PrTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

