namespace WMS_WEBAPI.DTOs
{
    public class BaseRouteCreateDto
    {       
        public decimal Quantity { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public string? ScannedBarcode { get; set; }
        public int? SourceWarehouse { get; set; }
        public int? TargetWarehouse { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
    }
}