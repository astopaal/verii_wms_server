using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto);
        Task<ApiResponse<IEnumerable<NotificationDto>>> CreateForUsersAsync(CreateNotificationDto dto);
        Task<ApiResponse<PagedResponse<NotificationDto>>> GetPagedByRecipientUserIdAsync(long userId, PagedRequest request);
        Task<ApiResponse<bool>> MarkAsReadAsync(long id);
        Task<ApiResponse<bool>> MarkAsReadBulkAsync(List<long> ids);
        Task<ApiResponse<bool>> MarkAllAsReadAsync(long userId);
        

        /// <summary>
        /// Creates notification entities for terminal lines without transaction or SaveChanges.
        /// Caller is responsible for adding to UnitOfWork and committing transaction.
        /// </summary>
        List<Notification> CreateNotificationsForTerminalLines<TTerminalLine>(
            IEnumerable<TTerminalLine> terminalLines,
            string orderNumber,
            string entityType,
            string terminalActionCode,
            string titleLocalizationKey,
            string messageLocalizationKey) where TTerminalLine : class;

        /// <summary>
        /// Creates notifications for terminal lines and adds them to UnitOfWork.
        /// After transaction is committed, call PublishSignalRNotificationsForCreatedNotificationsAsync to publish SignalR notifications.
        /// </summary>
        Task<List<Notification>> CreateAndAddNotificationsForTerminalLinesAsync<TTerminalLine>(
            IEnumerable<TTerminalLine> terminalLines,
            string orderNumber,
            string entityType,
            string terminalActionCode,
            string titleLocalizationKey,
            string messageLocalizationKey) where TTerminalLine : class;

        /// <summary>
        /// Publishes SignalR notifications for the given notification entities.
        /// Should be called after transaction is committed.
        /// </summary>
        Task PublishSignalRNotificationsAsync(IEnumerable<Notification> notifications);

        /// <summary>
        /// Publishes SignalR notifications for created notification entities.
        /// Fetches notifications from database (to get IDs) and publishes them via SignalR.
        /// Should be called after transaction is committed.
        /// </summary>
        Task PublishSignalRNotificationsForCreatedNotificationsAsync(List<Notification> notifications);
    }
}
