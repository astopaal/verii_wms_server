using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserDetailController : ControllerBase
    {
        private readonly IUserDetailService _userDetailService;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileUploadService _fileUploadService;
        private readonly UnitOfWork.IUnitOfWork _unitOfWork;

        public UserDetailController(IUserDetailService userDetailService, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IFileUploadService fileUploadService, UnitOfWork.IUnitOfWork unitOfWork)
        {
            _userDetailService = userDetailService;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get user detail by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetById(long id)
        {
            var result = await _userDetailService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get user detail by User ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetByUserId(long userId)
        {
            var result = await _userDetailService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get user detail for current user
        /// </summary>
        [HttpGet("current")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetCurrentUserDetail()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            var result = await _userDetailService.GetByUserIdAsync(userIdLong);
            
            // If not found, return 200 with null data instead of 404 (frontend can handle this better)
            if (result.StatusCode == 404)
            {
                return Ok(ApiResponse<UserDetailDto>.SuccessResult(null!, "User detail not found. You can create one."));
            }
            
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all user details
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDetailDto>>>> GetAll()
        {
            var result = await _userDetailService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get paginated user details
        /// </summary>
        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<UserDetailDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _userDetailService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create a new user detail
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> Create([FromBody] CreateUserDetailDto dto)
        {
            var result = await _userDetailService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update user detail
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> Update(long id, [FromBody] UpdateUserDetailDto dto)
        {
            var result = await _userDetailService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update current user's detail (creates if doesn't exist)
        /// </summary>
        [HttpPut("current")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> UpdateCurrent([FromBody] UpdateUserDetailDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<UserDetailDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            // Check if UserDetail exists by querying directly with AsNoTracking (to avoid tracking conflicts)
            var existingDetail = await _unitOfWork.UserDetails
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userIdLong && !x.IsDeleted);

            if (existingDetail == null)
            {
                // UserDetail doesn't exist, create a new one
                var createDto = new CreateUserDetailDto
                {
                    UserId = userIdLong,
                    ProfilePictureUrl = dto.ProfilePictureUrl,
                    Height = dto.Height,
                    Weight = dto.Weight,
                    Description = dto.Description,
                    Gender = dto.Gender
                };
                var createResult = await _userDetailService.CreateAsync(createDto);
                return StatusCode(createResult.StatusCode, createResult);
            }

            // UserDetail exists, update it using the ID
            var result = await _userDetailService.UpdateAsync(existingDetail.Id, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete user detail (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _userDetailService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Upload profile picture for current user
        /// </summary>
        [HttpPost("upload-profile-picture")]
        public async Task<ActionResult<ApiResponse<string>>> UploadProfilePicture(IFormFile file)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<string>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            // Check if user has existing profile picture and delete it before uploading new one
            var existingUserDetail = await _userDetailService.GetByUserIdAsync(userIdLong);
            if (existingUserDetail.Success && existingUserDetail.Data != null && !string.IsNullOrEmpty(existingUserDetail.Data.ProfilePictureUrl))
            {
                // Delete old profile picture file
                await _fileUploadService.DeleteProfilePictureAsync(existingUserDetail.Data.ProfilePictureUrl);
            }

            // Upload new profile picture
            var uploadResult = await _fileUploadService.UploadProfilePictureAsync(file, userIdLong);
            if (!uploadResult.Success)
            {
                return StatusCode(uploadResult.StatusCode, uploadResult);
            }

            // Update or create UserDetail with the new profile picture URL
            if (existingUserDetail.Success && existingUserDetail.Data != null)
            {
                // Update existing
                var updateDto = new UpdateUserDetailDto
                {
                    ProfilePictureUrl = uploadResult.Data
                };
                var updateResult = await _userDetailService.UpdateAsync(existingUserDetail.Data.Id, updateDto);
                return StatusCode(updateResult.StatusCode, ApiResponse<string>.SuccessResult(uploadResult.Data!, _localizationService.GetLocalizedString("ProfilePictureUploadedSuccessfully")));
            }
            else
            {
                // Create new
                var createDto = new CreateUserDetailDto
                {
                    UserId = userIdLong,
                    ProfilePictureUrl = uploadResult.Data
                };
                var createResult = await _userDetailService.CreateAsync(createDto);
                return StatusCode(createResult.StatusCode, ApiResponse<string>.SuccessResult(uploadResult.Data!, _localizationService.GetLocalizedString("ProfilePictureUploadedSuccessfully")));
            }
        }

        /// <summary>
        /// Check if profile picture file exists (for debugging)
        /// </summary>
        [HttpGet("check-profile-picture/{userId}/{fileName}")]
        public ActionResult<ApiResponse<object>> CheckProfilePicture(long userId, string fileName)
        {
            try
            {
                var webRootPath = _httpContextAccessor.HttpContext?.RequestServices
                    .GetRequiredService<IWebHostEnvironment>().WebRootPath;
                
                if (string.IsNullOrEmpty(webRootPath))
                {
                    webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var filePath = Path.Combine(webRootPath, "uploads", "user-profiles", userId.ToString(), fileName);
                var exists = System.IO.File.Exists(filePath);
                
                var result = new
                {
                    filePath,
                    exists,
                    webRootPath,
                    fileSize = exists ? new FileInfo(filePath).Length : 0
                };

                return Ok(ApiResponse<object>.SuccessResult(result, exists ? "File exists" : "File not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Error", ex.Message, 500));
            }
        }

        /// <summary>
        /// Test endpoint for User ID 14 (for debugging)
        /// </summary>
        [HttpGet("test/user/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<object>>> TestUserDetail(long userId)
        {
            try
            {
                // Check if user exists
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return Ok(ApiResponse<object>.ErrorResult("User not found", $"User with ID {userId} not found", 404));
                }

                // Get all UserDetail records for this user
                var allUserDetails = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .Where(ud => ud.UserId == userId)
                    .ToListAsync();

                // Get active UserDetail
                var activeDetail = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

                // Test the exact query used by GetByUserIdAsync
                var queryResult = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

                var testResult = new
                {
                    userId = userId,
                    userInfo = new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        username = user.Username,
                        isDeleted = user.IsDeleted
                    },
                    allUserDetailsCount = allUserDetails.Count,
                    allUserDetails = allUserDetails.Select(ud => new
                    {
                        id = ud.Id,
                        userId = ud.UserId,
                        isDeleted = ud.IsDeleted,
                        deletedDate = ud.DeletedDate,
                        profilePictureUrl = ud.ProfilePictureUrl,
                        height = ud.Height,
                        weight = ud.Weight,
                        gender = ud.Gender,
                        description = ud.Description,
                        createdDate = ud.CreatedDate,
                        updatedDate = ud.UpdatedDate
                    }).ToList(),
                    activeUserDetail = activeDetail != null ? new
                    {
                        id = activeDetail.Id,
                        userId = activeDetail.UserId,
                        isDeleted = activeDetail.IsDeleted,
                        profilePictureUrl = activeDetail.ProfilePictureUrl,
                        height = activeDetail.Height,
                        weight = activeDetail.Weight,
                        gender = activeDetail.Gender,
                        description = activeDetail.Description
                    } : null,
                    queryResult = queryResult != null ? new
                    {
                        id = queryResult.Id,
                        userId = queryResult.UserId,
                        isDeleted = queryResult.IsDeleted
                    } : null,
                    analysis = new
                    {
                        hasUser = user != null,
                        hasUserDetails = allUserDetails.Count > 0,
                        hasActiveUserDetail = activeDetail != null,
                        willReturn404 = queryResult == null,
                        reason = queryResult == null 
                            ? (allUserDetails.Count == 0 
                                ? "No UserDetail records exist for this user" 
                                : "All UserDetail records are marked as deleted (IsDeleted = true)")
                            : "UserDetail found, should return 200"
                    }
                };

                return Ok(ApiResponse<object>.SuccessResult(testResult, "Test completed"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Error", ex.Message, 500));
            }
        }

        /// <summary>
        /// Debug endpoint to check user detail status
        /// </summary>
        [HttpGet("debug/current")]
        public async Task<ActionResult<ApiResponse<object>>> DebugCurrentUserDetail()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            try
            {
                // Get all user details for this user (including deleted ones)
                var allDetails = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .Where(x => x.UserId == userIdLong)
                    .ToListAsync();

                // Get non-deleted user detail
                var activeDetail = await _unitOfWork.UserDetails
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.UserId == userIdLong && !x.IsDeleted);

                var debugInfo = new
                {
                    userIdFromToken = userId,
                    userIdParsed = userIdLong,
                    allUserDetailsCount = allDetails.Count,
                    allUserDetails = allDetails.Select(ud => new
                    {
                        id = ud.Id,
                        userId = ud.UserId,
                        isDeleted = ud.IsDeleted,
                        deletedDate = ud.DeletedDate,
                        profilePictureUrl = ud.ProfilePictureUrl
                    }).ToList(),
                    activeUserDetail = activeDetail != null ? new
                    {
                        id = activeDetail.Id,
                        userId = activeDetail.UserId,
                        isDeleted = activeDetail.IsDeleted,
                        profilePictureUrl = activeDetail.ProfilePictureUrl,
                        height = activeDetail.Height,
                        weight = activeDetail.Weight,
                        gender = activeDetail.Gender
                    } : null
                };

                return Ok(ApiResponse<object>.SuccessResult(debugInfo, "Debug information retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Error", ex.Message, 500));
            }
        }

        /// <summary>
        /// Delete profile picture for current user
        /// </summary>
        [HttpDelete("delete-profile-picture")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProfilePicture()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            var userDetail = await _userDetailService.GetByUserIdAsync(userIdLong);
            if (!userDetail.Success || userDetail.Data == null)
            {
                return StatusCode(404, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDetailNotFound"),
                    _localizationService.GetLocalizedString("UserDetailNotFound"),
                    404));
            }

            // Delete file if exists
            if (!string.IsNullOrEmpty(userDetail.Data.ProfilePictureUrl))
            {
                await _fileUploadService.DeleteProfilePictureAsync(userDetail.Data.ProfilePictureUrl);
            }

            // Update UserDetail to remove profile picture URL
            var updateDto = new UpdateUserDetailDto
            {
                ProfilePictureUrl = null
            };
            var result = await _userDetailService.UpdateAsync(userDetail.Data.Id, updateDto);
            return StatusCode(result.StatusCode, ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ProfilePictureDeletedSuccessfully")));
        }
    }
}
