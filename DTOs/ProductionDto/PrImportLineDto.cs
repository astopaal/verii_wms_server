using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long? LineId { get; set; }
    }

    public class CreatePrImportLineDto : BaseImportLineCreateDto
    {
        public long HeaderId { get; set; }
        public long? LineId { get; set; }
    }

    public class UpdatePrImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
    }
}
