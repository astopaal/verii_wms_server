using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_USER_AUTHORITY")]
    public class UserAuthority : BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
    }
}