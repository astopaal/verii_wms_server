using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WiHeaderDto : BaseHeaderEntityDto
    {
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string InboundType { get; set; } = string.Empty;
        public string? AccountCode { get; set; }
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
        public byte Type { get; set; }
    }

    public class CreateWiHeaderDto : BaseHeaderCreateDto
    {
        [Required, StringLength(50)]
        public string DocumentNo { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; }

        [Required, StringLength(10)]
        public string InboundType { get; set; } = string.Empty;

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

    public class UpdateWiHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(50)]
        public string? DocumentNo { get; set; }

        public DateTime? DocumentDate { get; set; }

        [StringLength(10)]
        public string? InboundType { get; set; }

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

    public class WiAssignedOrderLinesDto
    {
        public IEnumerable<WiLineDto> Lines { get; set; } = Array.Empty<WiLineDto>();
        public IEnumerable<WiLineSerialDto> LineSerials { get; set; } = Array.Empty<WiLineSerialDto>();
        public IEnumerable<WiImportLineDto> ImportLines { get; set; } = Array.Empty<WiImportLineDto>();
        public IEnumerable<WiRouteDto> Routes { get; set; } = Array.Empty<WiRouteDto>();
    }
}
