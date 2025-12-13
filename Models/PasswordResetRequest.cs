using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WMS_WEBAPI.Models
{
    [Table("RII_PASSWORD_RESET_REQUEST")]
    public class PasswordResetRequest : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [Required]
        [StringLength(128)]
        public string TokenHash { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        [StringLength(100)]
        public string? RequestIp { get; set; }
        [StringLength(500)]
        public string? UserAgent { get; set; }
    }
}
