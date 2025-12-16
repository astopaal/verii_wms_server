using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtTerminalLineService : IWtTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WtTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtTerminalLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);  
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtTerminalLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
                }

                var dto = _mapper.Map<WtTerminalLineDto>(entity);
                return ApiResponse<WtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByTerminalIdAsync(long terminalId)
        {
            try
            {
                var entities = await _unitOfWork.WtTerminalLines
                    .FindAsync(x => x.TerminalUserId == terminalId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WtTerminalLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.WtTerminalLines
                    .FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtTerminalLineDto>> CreateAsync(CreateWtTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtTerminalLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;

                await _unitOfWork.WtTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtTerminalLineDto>(entity);
                return ApiResponse<WtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtTerminalLineDto>> UpdateAsync(long id, UpdateWtTerminalLineDto dto)
        {
            try
            {
                var entity = await _unitOfWork.WtTerminalLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.Now;

                _unitOfWork.WtTerminalLines
                    .Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedDto = _mapper.Map<WtTerminalLineDto>(entity);
                return ApiResponse<WtTerminalLineDto>.SuccessResult(updatedDto, _localizationService.GetLocalizedString("WtTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WtTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
                }

                await _unitOfWork.WtTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
