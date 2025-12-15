using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WtHeaderDto : BaseHeaderEntityDto
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

    public class CreateWtHeaderDto : BaseHeaderCreateDto
    {
        [StringLength(50)]
        public string DocumentNo { get; set; } = string.Empty;

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

    public class UpdateWtHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(50)]
        public string? DocumentNo { get; set; }

        public DateTime? DocumentDate { get; set; }

        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        [StringLength(10)]
        public string? Priority { get; set; }

        public byte? Type { get; set; }
    }

    public class WtAssignedTransferOrderLinesDto
    {
        public IEnumerable<WtLineDto> Lines { get; set; } = Array.Empty<WtLineDto>();
        public IEnumerable<WtLineSerialDto> LineSerials { get; set; } = Array.Empty<WtLineSerialDto>();
        public IEnumerable<WtImportLineDto> ImportLines { get; set; } = Array.Empty<WtImportLineDto>();
        public IEnumerable<WtRouteDto> Routes { get; set; } = Array.Empty<WtRouteDto>();
    }
}
