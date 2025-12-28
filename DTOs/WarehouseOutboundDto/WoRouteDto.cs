using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WoRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string YapKod { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PackageLineId { get; set; }
        public string? PackageNo { get; set; }
        public long? PackageHeaderId { get; set; }
    }

    public class CreateWoRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? StockName { get; set; }
        public string YapKod { get; set; } = string.Empty;
        public string? YapAcik { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateWoRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? Description { get; set; }
    }
}
