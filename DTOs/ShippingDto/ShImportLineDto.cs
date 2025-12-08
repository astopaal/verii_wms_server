using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long? LineId { get; set; }
    }

    public class CreateShImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
        public long? LineId { get; set; }
    }

    public class UpdateShImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
    }
}
