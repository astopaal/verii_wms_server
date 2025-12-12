using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserAuthorityService : IUserAuthorityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public UserAuthorityService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<UserAuthorityDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.UserAuthorities.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<UserAuthorityDto>>(entities);
                return ApiResponse<IEnumerable<UserAuthorityDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("UserAuthorityRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserAuthorityDto>>.ErrorResult(_localizationService.GetLocalizedString("Error_GetAll"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserAuthorityDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.UserAuthorities.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
                }

                var dto = _mapper.Map<UserAuthorityDto>(entity);
                return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("Error_GetById"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserAuthorityDto>> CreateAsync(CreateUserAuthorityDto createDto)
        {
            try
            {
                var entity = _mapper.Map<UserAuthority>(createDto);
                await _unitOfWork.UserAuthorities.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<UserAuthorityDto>(entity);
                return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Create"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserAuthorityDto>> UpdateAsync(long id, UpdateUserAuthorityDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.UserAuthorities.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                _unitOfWork.UserAuthorities.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<UserAuthorityDto>(entity);
                return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Update"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.UserAuthorities.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
                }

                await _unitOfWork.UserAuthorities.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("UserAuthorityDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("Error_Delete"), ex.Message ?? string.Empty, 500);
            }
        }

        
    }
}
