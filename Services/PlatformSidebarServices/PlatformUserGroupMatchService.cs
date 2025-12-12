using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PlatformUserGroupMatchService : IPlatformUserGroupMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PlatformUserGroupMatchService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetAllAsync()
        {
            try
            {
                var matches = await _unitOfWork.PlatformUserGroupMatches.GetAllAsync();
                var result = _mapper.Map<IEnumerable<PlatformUserGroupMatchDto>>(matches);
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PlatformUserGroupMatchDto>> GetByIdAsync(long id)
        {
            try
            {
                var match = await _unitOfWork.PlatformUserGroupMatches.GetByIdAsync(id);
                if (match == null)
                {
                    var nf = _localizationService.GetLocalizedString("PlatformUserGroupMatchNotFound");
                    return ApiResponse<PlatformUserGroupMatchDto>.ErrorResult(nf, nf, 404);
                }
                
                var result = _mapper.Map<PlatformUserGroupMatchDto>(match);
                return ApiResponse<PlatformUserGroupMatchDto>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PlatformUserGroupMatchDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievalError"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }

        public async Task<ApiResponse<PlatformUserGroupMatchDto>> CreateAsync(CreatePlatformUserGroupMatchDto createDto)
        {
            try
            {
                var match = _mapper.Map<PlatformUserGroupMatch>(createDto);
                var createdMatch = await _unitOfWork.PlatformUserGroupMatches.AddAsync(match);
                await _unitOfWork.SaveChangesAsync();
                
                var result = _mapper.Map<PlatformUserGroupMatchDto>(createdMatch);
                return ApiResponse<PlatformUserGroupMatchDto>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PlatformUserGroupMatchDto>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PlatformUserGroupMatchDto>> UpdateAsync(long id, UpdatePlatformUserGroupMatchDto updateDto)
        {
            try
            {
                var existingMatch = await _unitOfWork.PlatformUserGroupMatches.GetByIdAsync(id);
                if (existingMatch == null)
                {
                    var nf = _localizationService.GetLocalizedString("PlatformUserGroupMatchNotFound");
                    return ApiResponse<PlatformUserGroupMatchDto>.ErrorResult(nf, nf, 404);
                }

                if (updateDto.UserId.HasValue)
                    existingMatch.UserId = updateDto.UserId.Value;

                if (updateDto.GroupCode != null)
                    existingMatch.GroupCode = updateDto.GroupCode;

                _unitOfWork.PlatformUserGroupMatches.Update(existingMatch);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<PlatformUserGroupMatchDto>(existingMatch);
                return ApiResponse<PlatformUserGroupMatchDto>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PlatformUserGroupMatchDto>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var match = await _unitOfWork.PlatformUserGroupMatches.GetByIdAsync(id);
                if (match == null)
                {
                    var nf = _localizationService.GetLocalizedString("PlatformUserGroupMatchNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                await _unitOfWork.PlatformUserGroupMatches.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PlatformUserGroupMatchDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var matches = await _unitOfWork.PlatformUserGroupMatches.FindAsync(m => m.UserId == userId);
                var result = _mapper.Map<IEnumerable<PlatformUserGroupMatchDto>>(matches);
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetByGroupCodeAsync(string groupCode)
        {
            try
            {
                var matches = await _unitOfWork.PlatformUserGroupMatches.FindAsync(m => m.GroupCode == groupCode);
                var result = _mapper.Map<IEnumerable<PlatformUserGroupMatchDto>>(matches);
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>.ErrorResult(_localizationService.GetLocalizedString("PlatformUserGroupMatchRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }



    }
}
