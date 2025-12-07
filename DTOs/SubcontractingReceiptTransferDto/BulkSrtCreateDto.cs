using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class CreateSrtLineWithKeyDto
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

    public class CreateSrtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateSrtRouteWithLineKeyDto
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

    public class CreateSrtImportLineWithKeysDto
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

    public class CreateSrtTerminalLineWithUserDto
    {
        public long TerminalUserId { get; set; }
    }

    public class BulkCreateSrtRequestDto
    {
        public CreateSrtHeaderDto Header { get; set; } = null!;
        public List<CreateSrtLineWithKeyDto>? Lines { get; set; }
        public List<CreateSrtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateSrtRouteWithLineKeyDto>? Routes { get; set; }
        public List<CreateSrtImportLineWithKeysDto>? ImportLines { get; set; }
    }

    public class GenerateSubcontractingReceiptOrderRequestDto
    {
        public CreateSrtHeaderDto Header { get; set; } = null!;
        public List<CreateSrtLineWithKeyDto>? Lines { get; set; }
        public List<CreateSrtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateSrtTerminalLineWithUserDto>? TerminalLines { get; set; }
    }
}

