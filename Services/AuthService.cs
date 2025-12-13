using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILocalizationService _localizationService;
        private readonly WmsDbContext _context;
        private readonly IHubContext<WMS_WEBAPI.Hubs.AuthHub> _hubContext;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, ILocalizationService localizationService, WmsDbContext context, IHubContext<WMS_WEBAPI.Hubs.AuthHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _localizationService = localizationService;
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<ApiResponse<UserDto>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var query = _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation);
                var user = await query.FirstOrDefaultAsync(u => u.Username == username);
                
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<UserDto>.ErrorResult(nf, nf, 404);
                }

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation).FirstOrDefaultAsync(u => u.Id == id);
                
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<UserDto>.ErrorResult(nf, nf, 404);
                }

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUserResponse = await GetUserByUsernameAsync(registerDto.Username);
                if (existingUserResponse.Success)
                {
                    var msg = _localizationService.GetLocalizedString("AuthUserAlreadyExists");
                    return ApiResponse<UserDto>.ErrorResult(msg, msg, 400);
                }

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRegisteredSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthRegistrationFailed"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginRequest request)
        {
            try
            {
                var loginDto = new LoginDto
                {
                    Username = request.Email,
                    Password = request.Password
                };
                // Email veya username ile kullanıcı arama
                var user = await _unitOfWork.Users.AsQueryable().FirstOrDefaultAsync(u => u.Username == loginDto.Username || u.Email == loginDto.Username);
                
                if (user == null)
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    return ApiResponse<string>.ErrorResult(msg, msg, 401);
                }
                
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    return ApiResponse<string>.ErrorResult(msg, msg, 401);
                }

                var tokenResponse = _jwtService.GenerateToken(user);
                if (!tokenResponse.Success)
                {
                    return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("Error.User.LoginFailed"), tokenResponse.Message ?? string.Empty, 500);
                }
                var token = tokenResponse.Data!;

                var activeSession = _context.Set<UserSession>().FirstOrDefault(s => s.UserId == user.Id && s.RevokedAt == null);
                if (activeSession != null)
                {
                    activeSession.RevokedAt = DateTime.UtcNow;
                    _context.SaveChanges();
                    await WMS_WEBAPI.Hubs.AuthHub.ForceLogoutUser(_hubContext, user.Id.ToString());
                }

                var session = new UserSession
                {
                    UserId = user.Id,
                    SessionId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Token = ComputeSha256Hash(token),
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Set<UserSession>().Add(session);
                _context.SaveChanges();
                
                return ApiResponse<string>.SuccessResult(token, _localizationService.GetLocalizedString("Success.User.LoginSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("Error.User.LoginFailed"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation).ToListAsync();
                var dtos = users.Select(MapToUserDto).ToList();
                return ApiResponse<IEnumerable<UserDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("DataRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<string>> RequestPasswordResetAsync(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable().FirstOrDefaultAsync(u => u.Email == request.Email);
                var token = Guid.NewGuid().ToString("N");
                var tokenHash = ComputeSha256Hash(token);
                var expiresAt = DateTime.UtcNow.AddMinutes(30);

                if (user != null)
                {
                    var reset = new PasswordResetRequest
                    {
                        UserId = user.Id,
                        TokenHash = tokenHash,
                        ExpiresAt = expiresAt,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _context.Set<PasswordResetRequest>().Add(reset);
                    await _context.SaveChangesAsync();
                }

                var msg = _localizationService.GetLocalizedString("OperationSuccessful");
                return ApiResponse<string>.SuccessResult(token, msg);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var tokenHash = ComputeSha256Hash(request.Token);
                var now = DateTime.UtcNow;

                var reset = await _context.Set<PasswordResetRequest>()
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.TokenHash == tokenHash && r.UsedAt == null && r.ExpiresAt > now && !r.IsDeleted);

                if (reset == null || reset.User == null)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), _localizationService.GetLocalizedString("ValidationError"), 400);
                }

                reset.UsedAt = now;
                reset.UpdatedDate = now;

                reset.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                reset.User.UpdatedDate = now;

                await _context.SaveChangesAsync();

                await InvalidateUserSessionsAsync(reset.User.Id);

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(long userId, ChangePasswordRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable().FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                await InvalidateUserSessionsAsync(user.Id);

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private async Task InvalidateUserSessionsAsync(long userId)
        {
            var sessions = await _context.Set<UserSession>()
                .Where(s => s.UserId == userId && s.RevokedAt == null)
                .ToListAsync();

            if (sessions.Count > 0)
            {
                var now = DateTime.UtcNow;
                foreach (var s in sessions)
                {
                    s.RevokedAt = now;
                    s.UpdatedDate = now;
                }
                await _context.SaveChangesAsync();
                await WMS_WEBAPI.Hubs.AuthHub.ForceLogoutUser(_hubContext, userId.ToString());
            }
        }

        static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.RoleNavigation?.Title ?? "User",
                IsEmailConfirmed = user.IsEmailConfirmed,
                LastLoginDate = user.LastLoginDate,
                FullName = user.FullName
            };
        }
    }
}
