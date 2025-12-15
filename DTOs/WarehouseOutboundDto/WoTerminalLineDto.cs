using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WoTerminalLineDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
    }

    public class CreateWoTerminalLineDto : BaseTerminalLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateWoTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}
