using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WoHeaderDto : BaseHeaderEntityDto
    {
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string OutboundType { get; set; } = string.Empty;
        public string? AccountCode { get; set; }
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
        public byte Type { get; set; }
    }

    public class CreateWoHeaderDto : BaseHeaderCreateDto
    {
        [Required, StringLength(50)]
        public string DocumentNo { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; }

        [Required, StringLength(10)]
        public string OutboundType { get; set; } = string.Empty;

        [StringLength(20)]
        public string? AccountCode { get; set; }

        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        [Required]
        public byte Type { get; set; }
    }

    public class UpdateWoHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(50)]
        public string? DocumentNo { get; set; }

        public DateTime? DocumentDate { get; set; }

        [StringLength(10)]
        public string? OutboundType { get; set; }

        [StringLength(20)]
        public string? AccountCode { get; set; }

        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        public byte? Type { get; set; }
    }

    public class WoAssignedOrderLinesDto
    {
        public IEnumerable<WoLineDto> Lines { get; set; } = Array.Empty<WoLineDto>();
        public IEnumerable<WoLineSerialDto> LineSerials { get; set; } = Array.Empty<WoLineSerialDto>();
        public IEnumerable<WoImportLineDto> ImportLines { get; set; } = Array.Empty<WoImportLineDto>();
        public IEnumerable<WoRouteDto> Routes { get; set; } = Array.Empty<WoRouteDto>();
    }
}
