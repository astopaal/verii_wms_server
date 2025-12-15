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
