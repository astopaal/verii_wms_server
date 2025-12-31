using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class UserAuthorityDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
        
        // Full user information properties
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreateUserAuthorityDto
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
    }

    public class UpdateUserAuthorityDto
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
    }
}
