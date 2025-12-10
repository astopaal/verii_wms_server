using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrLineSerialDto : BaseLineSerialEntityDto
    {
        public long LineId { get; set; }
    }

    public class CreatePrLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long LineId { get; set; }
    }

    public class UpdatePrLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
    }
}
