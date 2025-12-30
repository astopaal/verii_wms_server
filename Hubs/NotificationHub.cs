using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WMS_WEBAPI.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // Connection tracking: userId -> connectionId
        private static readonly Dictionary<string, string> _userConnections = new();
        // Connection tracking: connectionId -> userId
        private static readonly Dictionary<string, string> _connectionUsers = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId != null)
            {
                // Yeni bağlantıyı kaydet
                _userConnections[userId] = Context.ConnectionId;
                _connectionUsers[Context.ConnectionId] = userId;
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectionUsers.TryGetValue(Context.ConnectionId, out var userId))
            {
                _userConnections.Remove(userId);
                _connectionUsers.Remove(Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Sends notification to a specific user by user ID.
        /// Works for both web users and terminal users (both are normal User IDs).
        /// Uses connection tracking for reliable delivery.
        /// </summary>
        public static async Task SendNotificationToUser(IHubContext<NotificationHub> hubContext, string userId, object payload)
        {
            try
            {
                // First try using SignalR's built-in User method (works if IUserIdProvider is configured)
                // This uses JWT token's NameIdentifier claim to match the user
                await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
                
                // Also try direct connection ID lookup for reliability
                // This ensures delivery even if SignalR's User mapping doesn't work
                if (_userConnections.TryGetValue(userId, out var connectionId))
                {
                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", payload);
                }
                else
                {
                    // Log if user is not connected (for debugging)
                    System.Diagnostics.Debug.WriteLine($"User {userId} is not connected to NotificationHub. Active connections: {_userConnections.Count}");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw
                System.Diagnostics.Debug.WriteLine($"Error sending notification to user {userId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends notification to all connected users
        /// </summary>
        public static async Task SendNotificationToAll(IHubContext<NotificationHub> hubContext, object payload)
        {
            await hubContext.Clients.All.SendAsync("ReceiveNotification", payload);
        }
    }
}
