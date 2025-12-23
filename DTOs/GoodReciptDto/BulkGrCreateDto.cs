using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class CreateGrLineWithKeyDto : BaseLineCreateDto
    {
        [Required]
        public string ClientKey { get; set; } = null!;

    }

    public class CreateGrImportLWithLineKeyDto
    {
        public string? LineClientKey { get; set; }

        [Required]
        public string ClientKey { get; set; } = null!;

        public string StockCode { get; set; } = null!;
        public string? StockName { get; set; }
        public string YapKod { get; set; } = string.Empty;
        public string? YapAcik { get; set; }
        public string? Unit { get; set; }
        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description1 { get; set; }
        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description2 { get; set; }
    }

    public class CreateGrImportSerialLineWithImportLineKeyDto
    {
        [Required]
        public string ImportLineClientKey { get; set; } = null!;

        public string SerialNo { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
    }

    public class CreateGrRouteWithImportLineKeyDto
    {
        [Required]
        public string ImportLineClientKey { get; set; } = null!;

        public string ScannedBarcode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string? StockCode { get; set; }
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public int? SourceWarehouse { get; set; }
        public int? TargetWarehouse { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
    }

    public class CreateGrImportDocumentSimpleDto
    {
        public byte[] Base64 { get; set; } = null!;
    }

    public class BulkCreateGrRequestDto
    {
        public CreateGrHeaderDto Header { get; set; } = null!;

        public List<CreateGrImportDocumentSimpleDto>? Documents { get; set; }
        public List<CreateGrLineWithKeyDto>? Lines { get; set; }
        public List<CreateGrImportLWithLineKeyDto>? ImportLines { get; set; }
        public List<CreateGrImportSerialLineWithImportLineKeyDto>? SerialLines { get; set; }
        public List<CreateGrRouteWithImportLineKeyDto>? Routes { get; set; }
    }
}
