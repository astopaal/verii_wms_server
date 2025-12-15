using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShHeaderDto : BaseHeaderEntityDto
    {
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouse { get; set; }
        public string? TargetWarehouseName { get; set; }
        public byte Type { get; set; }
    }

    public class CreateShHeaderDto : BaseHeaderCreateDto
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
        [Required]
        public byte Type { get; set; }
    }

    public class UpdateShHeaderDto : BaseHeaderUpdateDto
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
        public byte? Type { get; set; }
    }

    public class ShAssignedOrderLinesDto
    {
        public IEnumerable<ShLineDto> Lines { get; set; } = Array.Empty<ShLineDto>();
        public IEnumerable<ShLineSerialDto> LineSerials { get; set; } = Array.Empty<ShLineSerialDto>();
        public IEnumerable<ShImportLineDto> ImportLines { get; set; } = Array.Empty<ShImportLineDto>();
        public IEnumerable<ShRouteDto> Routes { get; set; } = Array.Empty<ShRouteDto>();
    }

    public class CreateShLineWithKeyDto : BaseLineCreateDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }

        public int? OrderId { get; set; }
        public string? ErpLineReference { get; set; }
    }

    public class CreateShLineSerialWithLineKeyDto : BaseLineSerialCreateDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateShRouteWithLineKeyDto : BaseRouteCreateDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
        public string? ImportLineClientKey { get; set; }
        public Guid? ImportLineGroupGuid { get; set; }
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public string? Description { get; set; }
    }

    public class CreateShImportLineWithKeysDto : BaseImportLineCreateDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }

        public string? RouteClientKey { get; set; }
        public Guid? RouteGroupGuid { get; set; }

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

    public class CreateShTerminalLineWithUserDto : BaseTerminalLineCreateDto
    {
    }

    public class GenerateShipmentOrderRequestDto
    {
        public CreateShHeaderDto Header { get; set; } = null!;
        public List<CreateShLineWithKeyDto>? Lines { get; set; }
        public List<CreateShLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateShTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class BulkCreateShRequestDto
    {
        public CreateShHeaderDto Header { get; set; } = null!;
        public List<CreateShLineWithKeyDto>? Lines { get; set; }
        public List<CreateShLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateShRouteWithLineKeyDto>? Routes { get; set; }
        public List<CreateShImportLineWithKeysDto>? ImportLines { get; set; }
    }
}
