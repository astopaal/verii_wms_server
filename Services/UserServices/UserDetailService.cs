using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserDetailService : IUserDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IFileUploadService _fileUploadService;

        public UserDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _fileUploadService = fileUploadService;
        }

        public async Task<ApiResponse<UserDetailDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.UserDetails.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<UserDetailDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        404);
                }

                var dto = _mapper.Map<UserDetailDto>(entity);
                return ApiResponse<UserDetailDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<UserDetailDto>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entity = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .AsNoTracking()  // Add AsNoTracking to prevent tracking conflicts
                    .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

                if (entity == null)
                {
                    return ApiResponse<UserDetailDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        404);
                }

                var dto = _mapper.Map<UserDetailDto>(entity);
                return ApiResponse<UserDetailDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDetailDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<UserDetailDto>>(entities);
                return ApiResponse<IEnumerable<UserDetailDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDetailDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<PagedResponse<UserDetailDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.UserDetails.AsQueryable()
                    .Where(x => !x.IsDeleted);

                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<UserDetailDto>>(items);
                var result = new PagedResponse<UserDetailDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<UserDetailDto>>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<UserDetailDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailRetrievalError"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }

        public async Task<ApiResponse<UserDetailDto>> CreateAsync(CreateUserDetailDto dto)
        {
            try
            {
                // Check if user exists
                var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                if (user == null || user.IsDeleted)
                {
                    return ApiResponse<UserDetailDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserNotFound"),
                        _localizationService.GetLocalizedString("UserNotFound"),
                        404);
                }

                // Check if user detail already exists
                var existingDetail = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.UserId == dto.UserId && !x.IsDeleted);

                if (existingDetail != null)
                {
                    return ApiResponse<UserDetailDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserDetailAlreadyExists"),
                        _localizationService.GetLocalizedString("UserDetailAlreadyExists"),
                        400);
                }

                var entity = _mapper.Map<UserDetail>(dto);
                await _unitOfWork.UserDetails.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<UserDetailDto>(entity);
                return ApiResponse<UserDetailDto>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailCreationError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<UserDetailDto>> UpdateAsync(long id, UpdateUserDetailDto dto)
        {
            try
            {
                var entity = await _unitOfWork.UserDetails.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<UserDetailDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        404);
                }

                // If profile picture exists in entity and is being changed or removed, delete old one
                if (!string.IsNullOrEmpty(entity.ProfilePictureUrl) && 
                    (string.IsNullOrEmpty(dto.ProfilePictureUrl) || entity.ProfilePictureUrl != dto.ProfilePictureUrl))
                {
                    // Delete old profile picture before updating
                    await _fileUploadService.DeleteProfilePictureAsync(entity.ProfilePictureUrl);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                
                // Use Update which will handle tracking correctly
                _unitOfWork.UserDetails.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<UserDetailDto>(entity);
                return ApiResponse<UserDetailDto>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailUpdateError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.UserDetails.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        _localizationService.GetLocalizedString("UserDetailNotFound"),
                        404);
                }

                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.UtcNow;
                _unitOfWork.UserDetails.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("UserDetailDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailDeletionError"),
                    ex.Message,
                    500);
            }
        }
    }
}
