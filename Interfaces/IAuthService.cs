using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> GetUserByIdAsync(long id);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<IEnumerable<UserDto>>> GetActiveUsersAsync();
        Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterDto registerDto);
        Task<ApiResponse<string>> LoginAsync(LoginRequest loginDto);
        Task<ApiResponse<string>> RequestPasswordResetAsync(ForgotPasswordRequest request);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse<bool>> ChangePasswordAsync(long userId, ChangePasswordRequest request);
    }
}
