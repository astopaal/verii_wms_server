using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_USER_SESSION")]
    public class UserSession : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedAt { get; set; }

        [StringLength(100)]
        public string? IpAddress { get; set; }

        [StringLength(500)]
        public string? UserAgent { get; set; }

        [StringLength(100)]
        public string? DeviceInfo { get; set; }

        public bool IsActive => RevokedAt == null && CreatedAt.AddDays(30) > DateTime.UtcNow;
    }
}