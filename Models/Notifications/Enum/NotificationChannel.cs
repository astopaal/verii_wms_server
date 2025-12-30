using System;

namespace WMS_WEBAPI.Models
{
    /// <summary>
    /// Notification channel types
    /// Can be combined using bitwise OR (e.g., Web | Terminal)
    /// </summary>
    [Flags]
    public enum NotificationChannel : byte
    {
        Web = 1,
        Terminal = 2,
        Email = 4,
        SMS = 8
    }
}

