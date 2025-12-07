using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShRouteDto : BaseRouteEntityDto
    {
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public string? Description { get; set; }
    }

    public class CreateShRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateShRouteDto : BaseRouteUpdateDto
    {
        public long? LineId { get; set; }
        public string? StockCode { get; set; }
        public string? StockName { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public string? Description { get; set; }
    }
}
