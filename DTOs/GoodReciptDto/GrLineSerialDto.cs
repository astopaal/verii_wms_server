using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrLineSerialDto : BaseLineSerialEntityDto
    {
        public long? LineId { get; set; }
        public string? ClientKey { get; set; }
    }

    public class CreateGrLineSerialDto : BaseLineSerialCreateDto
    {
        public long? LineId { get; set; }
        public string? ClientKey { get; set; }
    }

    public class UpdateGrLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
        public string? ClientKey { get; set; }
    }
}