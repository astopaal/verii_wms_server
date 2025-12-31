using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_USER_DETAIL")]
    public class UserDetail : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // Kullanıcı resmi URL
        [StringLength(500)]
        public string? ProfilePictureUrl { get; set; }

        // Boy (cm cinsinden)
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; }

        // Kilo (kg cinsinden)
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        // Açıklama
        [StringLength(2000)]
        public string? Description { get; set; }

        // Cinsiyet
        public Gender? Gender { get; set; }
    }
}
