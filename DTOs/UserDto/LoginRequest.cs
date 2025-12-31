using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty; // Email veya Username olarak kullanılacak

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
