using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShTerminalLineDto
    {
        public long Id { get; set; }
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreateShTerminalLineDto
    {
        [Required]
        public long HeaderId { get; set; }
        [Required]
        public long TerminalUserId { get; set; }
    }

    public class UpdateShTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}
