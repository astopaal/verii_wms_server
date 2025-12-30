using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationController(INotificationService notificationService, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _notificationService = notificationService;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get paginated notifications for the current user (web user)
        /// </summary>
        [HttpPost("user/paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<NotificationDto>>>> GetPagedByCurrentUserId([FromBody] PagedRequest request)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<PagedResponse<NotificationDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            var result = await _notificationService.GetPagedByRecipientUserIdAsync(userIdLong, request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get paginated notifications for a specific user (web user)
        /// </summary>
        [HttpPost("user/{userId}/paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<NotificationDto>>>> GetPagedByUserId(long userId, [FromBody] PagedRequest request)
        {
            var result = await _notificationService.GetPagedByRecipientUserIdAsync(userId, request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mark all notifications as read for the current user
        /// </summary>
        [HttpPut("read/all")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAllAsRead()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    "User ID not found in token",
                    401));
            }

            var result = await _notificationService.MarkAllAsReadAsync(userIdLong);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        [HttpPut("{id}/read")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAsRead(long id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mark multiple notifications as read (bulk operation)
        /// </summary>
        [HttpPut("read/bulk")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAsReadBulk([FromBody] List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return StatusCode(400, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("InvalidModelState"),
                    "Notification IDs list cannot be empty",
                    400));
            }

            var result = await _notificationService.MarkAsReadBulkAsync(ids);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create a new notification
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<NotificationDto>>> Create([FromBody] CreateNotificationDto dto)
        {
            var result = await _notificationService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}

