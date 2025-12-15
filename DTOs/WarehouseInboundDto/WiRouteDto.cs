using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WiRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string YapKod { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CreateWiRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string YapKod { get; set; } = string.Empty;
        public string? YapAcik { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateWiRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? Description { get; set; }
    }
}
