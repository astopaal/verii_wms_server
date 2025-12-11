using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class AddPtImportBarcodeRequestDto
    {
        [Required]
        public long HeaderId { get; set; }

        public long? LineId { get; set; }

        [Required]
        [StringLength(75)]
        public string Barcode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }

        [StringLength(50)]
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }

        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
    }
}
