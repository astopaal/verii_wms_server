using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_NOTIFICATION")]
    public class Notification : BaseEntity
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Localization key for Title (e.g., "WtHeaderNotificationTitle")
        /// If set, Title will be localized when returned to frontend
        /// </summary>
        [MaxLength(200)]
        public string? TitleKey { get; set; }

        /// <summary>
        /// Localization key for Message (e.g., "WtHeaderNotificationMessage")
        /// If set, Message will be localized when returned to frontend
        /// </summary>
        [MaxLength(200)]
        public string? MessageKey { get; set; }

        public NotificationChannel Channel { get; set; }

        public NotificationSeverity? Severity { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; }

        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }
        [ForeignKey(nameof(RecipientUserId))]
        public virtual User? RecipientUser { get; set; }

        [MaxLength(100)]
        public string? RelatedEntityType { get; set; }

        public long? RelatedEntityId { get; set; }

        [MaxLength(250)]
        public string? ActionUrl { get; set; }

        [MaxLength(50)]
        public string? TerminalActionCode { get; set; }
    }
}
