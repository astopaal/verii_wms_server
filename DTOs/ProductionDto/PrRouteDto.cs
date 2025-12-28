using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public long? PackageLineId { get; set; }
        public string? PackageNo { get; set; }
        public long? PackageHeaderId { get; set; }
    }

    public class CreatePrRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
    }

    public class UpdatePrRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
    }
}
