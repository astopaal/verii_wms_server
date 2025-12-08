using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShLineSerialService : IShLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShLineSerials.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLineSerials.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShLineSerialNotFound"); return ApiResponse<ShLineSerialDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShLineSerialDto>(entity);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShLineSerials.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<ShLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> CreateAsync(CreateShLineSerialDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShLineSerial>(createDto);
                var created = await _unitOfWork.ShLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineSerialDto>(created);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> UpdateAsync(long id, UpdateShLineSerialDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShLineSerials.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShLineSerialNotFound"); return ApiResponse<ShLineSerialDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineSerialDto>(entity);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.ShLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

