using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public long? PackageLineId { get; set; }
        public string? PackageNo { get; set; }
        public long? PackageHeaderId { get; set; }
    }

    public class CreatePtRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
    }

    public class UpdatePtRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
    }
}