using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
    }

    public class CreateSitRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSitRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public int? Priority { get; set; }
        public string? Description { get; set; }
    }
}
