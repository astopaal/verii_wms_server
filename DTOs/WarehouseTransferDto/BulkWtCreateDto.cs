using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    // TR Line için istemci korelasyon anahtarı
    public class CreateWtLineWithKeyDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public int? OrderId { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
        public string? ErpOrderNo { get; set; }
        public string? ErpOrderId { get; set; }
        public string? ErpLineReference { get; set; }
        public string? Description { get; set; }
    }

    // Wt Route için line korelasyon anahtarı ve kendi korelasyon anahtarı
    public class CreateWtRouteWithLineKeyDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
        public string? ImportLineClientKey { get; set; }
        public Guid? ImportLineGroupGuid { get; set; }
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public decimal Quantity { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public short? SourceWarehouse { get; set; }
        public short? TargetWarehouse { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
        public string? Description { get; set; }
    }

    // Wt ImportLine için line ve route korelasyon anahtarları
    public class CreateWtImportLineWithKeysDto
    {
        // ImportLine kendi korelasyon anahtarı (route ile bağlamak için)
        public string? ClientKey { get; set; }
        public Guid? ClientGroupGuid { get; set; }

        // Line ile korelasyon
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }

        // Route ile korelasyon (opsiyonel)
        public string? RouteClientKey { get; set; }
        public Guid? RouteGroupGuid { get; set; }

        // Model alanları
        public string StockCode { get; set; } = string.Empty;
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

    // Toplu Wt oluşturma isteği
    public class BulkCreateWtRequestDto
    {
        public CreateWtHeaderDto Header { get; set; } = null!;

        public List<CreateWtLineWithKeyDto>? Lines { get; set; }
        public List<CreateWtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateWtRouteWithLineKeyDto>? Routes { get; set; }
        public List<CreateWtImportLineWithKeysDto>? ImportLines { get; set; }
    }

    // Tek depo transferi oluşturma isteği
    public class CreateWtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateWtTerminalLineWithUserDto
    {
        public long TerminalUserId { get; set; }
    }

    public class GenerateWarehouseTransferOrderRequestDto
    {
        public CreateWtHeaderDto Header { get; set; } = null!;
        public List<CreateWtLineWithKeyDto>? Lines { get; set; }
        public List<CreateWtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateWtTerminalLineWithUserDto>? TerminalLines { get; set; }
    }
}
