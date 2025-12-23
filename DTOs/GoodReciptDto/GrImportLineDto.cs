using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrImportLineDto : BaseImportLineEntityDto
    {
        public long? LineId { get; set; }
        public long HeaderId { get; set; }
    }

    public class GrImportLineWithRoutesDto : GrImportLineDto
    {
        public List<GrRouteDto> Routes { get; set; } = new List<GrRouteDto>();
    }

    public class CreateGrImportLineDto : BaseImportLineCreateDto
    {
        public long? LineId { get; set; }
        
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateGrImportLineDto : BaseImportLineUpdateDto
    {
        public long? LineId { get; set; }
        public long? HeaderId { get; set; }
    }
}

