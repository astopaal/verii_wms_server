namespace WMS_WEBAPI.DTOs
{
    public class BaseLineUpdateDto
    {
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? ErpOrderNo { get; set; }
        public string? ErpOrderId { get; set; }
        public string? Description { get; set; }
    }
}
