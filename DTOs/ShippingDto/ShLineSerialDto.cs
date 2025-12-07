using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShLineSerialDto : BaseLineSerialEntityDto
    {
        public long LineId { get; set; }
    }

    public class CreateShLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long LineId { get; set; }
    }

    public class UpdateShLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
    }
}
