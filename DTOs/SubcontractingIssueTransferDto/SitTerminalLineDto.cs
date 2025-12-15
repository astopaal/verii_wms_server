using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitTerminalLineDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
    }

    public class CreateSitTerminalLineDto : BaseTerminalLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateSitTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}
