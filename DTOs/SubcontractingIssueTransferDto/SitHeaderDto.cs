using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitHeaderDto : BaseHeaderEntityDto
    {
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouse { get; set; }
        public string? TargetWarehouseName { get; set; }
        public string? Priority { get; set; }
        public byte Type { get; set; }
    }

    public class CreateSitHeaderDto : BaseHeaderCreateDto
    {
        [Required]
        [StringLength(50)]
        public string DocumentNo { get; set; } = string.Empty;

        [Required]
        public DateTime DocumentDate { get; set; }

        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        [StringLength(10)]
        public string? Priority { get; set; }

        [Required]
        public byte Type { get; set; }
    }

    public class UpdateSitHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(20)]
        public string? CustomerCode { get; set; }

        public DateTime? DocumentDate { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        [StringLength(10)]
        public string? Priority { get; set; }

        public byte? Type { get; set; }
    }

    public class SitAssignedOrderLinesDto
    {
        public IEnumerable<SitLineDto> Lines { get; set; } = Array.Empty<SitLineDto>();
        public IEnumerable<SitLineSerialDto> LineSerials { get; set; } = Array.Empty<SitLineSerialDto>();
        public IEnumerable<SitImportLineDto> ImportLines { get; set; } = Array.Empty<SitImportLineDto>();
        public IEnumerable<SitRouteDto> Routes { get; set; } = Array.Empty<SitRouteDto>();
    }
}
