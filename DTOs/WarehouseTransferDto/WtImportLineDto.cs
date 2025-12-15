using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WtImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
    }

    public class CreateWtImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }
    }

    public class UpdateWtImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }
    }
}
