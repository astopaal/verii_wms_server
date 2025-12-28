using System.ComponentModel.DataAnnotations;
namespace WMS_WEBAPI.DTOs
{
    public class GrRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public string? Description { get; set; }
        public long? PackageLineId { get; set; }
        public string? PackageNo { get; set; }
        public long? PackageHeaderId { get; set; }
    }

    public class CreateGrRouteDto
    {
        [Required]
        public long ImportLineId { get; set; }
        [MaxLength(75)]
        public string ScannedBarcode { get; set; } = string.Empty;
        [Required]
        public decimal Quantity { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
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

    public class UpdateGrRouteDto
    {
        public long? ImportLineId { get; set; }
        public string? ScannedBarcode { get; set; }
        public decimal? Quantity { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
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
}
