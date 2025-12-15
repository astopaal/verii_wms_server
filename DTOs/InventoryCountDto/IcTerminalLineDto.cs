using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class IcTerminalLineDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
    }

    public class CreateIcTerminalLineDto : BaseTerminalLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateIcTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}
