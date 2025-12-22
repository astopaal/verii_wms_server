using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
    }

    public class CreateSrtImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }
    }

    public class UpdateSrtImportLineDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }

        [StringLength(35)]
        public string? StockCode { get; set; }

        [StringLength(30)]
        public string? Description1 { get; set; }

        [StringLength(50)]
        public string? Description2 { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
