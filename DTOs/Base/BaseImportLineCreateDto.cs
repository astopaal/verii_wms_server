namespace WMS_WEBAPI.DTOs
{
    public class BaseImportLineCreateDto
    {
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description { get; set; }
    }
}
