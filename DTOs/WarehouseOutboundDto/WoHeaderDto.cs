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
        public string? CustomerName { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouse { get; set; }
        public string? TargetWarehouseName { get; set; }
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

    public class CreateWoLineWithKeyDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public int? OrderId { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
        public string? ErpOrderNo { get; set; }
        public string? ErpOrderId { get; set; }
        public string? ErpLineReference { get; set; }
        public string? Description { get; set; }
    }

    public class CreateWoLineSerialWithLineKeyDto : BaseLineSerialCreateDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateWoRouteWithLineKeyDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
        public string? ImportLineClientKey { get; set; }
        public Guid? ImportLineGroupGuid { get; set; }
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public decimal Quantity { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public short? SourceWarehouse { get; set; }
        public short? TargetWarehouse { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
        public string? Description { get; set; }
    }

    public class CreateWoImportLineWithKeysDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }

        public string? RouteClientKey { get; set; }
        public Guid? RouteGroupGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public string? ScannedBarkod { get; set; }
        public string? ErpOrderNumber { get; set; }
        public string? ErpOrderNo { get; set; }
        public string? ErpOrderLineNumber { get; set; }
    }

    public class CreateWoTerminalLineWithUserDto
    {
        public long TerminalUserId { get; set; }
    }

    public class GenerateWarehouseOutboundOrderRequestDto
    {
        public CreateWoHeaderDto Header { get; set; } = null!;
        public List<CreateWoLineWithKeyDto>? Lines { get; set; }
        public List<CreateWoLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateWoTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class BulkCreateWoRequestDto
    {
        public CreateWoHeaderDto Header { get; set; } = null!;
        public List<CreateWoLineWithKeyDto>? Lines { get; set; }
        public List<CreateWoLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateWoRouteWithLineKeyDto>? Routes { get; set; }
        public List<CreateWoImportLineWithKeysDto>? ImportLines { get; set; }
    }
}
