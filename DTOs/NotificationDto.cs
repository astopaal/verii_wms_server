using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.DTOs
{
    public class NotificationDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? TitleKey { get; set; }
        public string? MessageKey { get; set; }
        public NotificationChannel Channel { get; set; }
        public NotificationSeverity? Severity { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public long? RecipientUserId { get; set; }
        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }

    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "Notification_Title_Required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notification_Message_Required")]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notification_Channel_Required")]
        public NotificationChannel Channel { get; set; }

        public NotificationSeverity? Severity { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }

        public List<long>? RecipientUserIds { get; set; }

        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }

    public class UpdateNotificationDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        public NotificationChannel? Channel { get; set; }
        public NotificationSeverity? Severity { get; set; }

        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }

        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }

    public class NotifyOrderOpenedDto
    {
        [Required(ErrorMessage = "Notification_EntityType_Required")]
        [StringLength(100)]
        public string EntityType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notification_EntityId_Required")]
        public long EntityId { get; set; }

        [Required(ErrorMessage = "Notification_OrderNumber_Required")]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        public long? RecipientUserId { get; set; }

        [StringLength(250)]
        public string? ActionUrl { get; set; }

        [StringLength(50)]
        public string? TerminalActionCode { get; set; }

        public NotificationChannel Channel { get; set; } = NotificationChannel.Web;
    }
}
