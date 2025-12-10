using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrHeaderDto : BaseHeaderEntityDto
    {
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? StockCode { get; set; }
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouse { get; set; }
        public string? TargetWarehouseName { get; set; }
    }

    public class CreatePrHeaderDto : BaseHeaderCreateDto
    {
        public string? CustomerCode { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }

    public class UpdatePrHeaderDto : BaseHeaderUpdateDto
    {
        public string? CustomerCode { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }
}
