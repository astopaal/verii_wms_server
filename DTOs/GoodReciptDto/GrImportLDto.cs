using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrImportLDto : BaseImportLineEntityDto
    {
        public long? LineId { get; set; }
        public long HeaderId { get; set; }
    }

    public class GrImportLWithRoutesDto : GrImportLDto
    {
        public List<GrRouteDto> Routes { get; set; } = new List<GrRouteDto>();
    }

    public class CreateGrImportLDto : BaseImportLineCreateDto
    {
        public long? LineId { get; set; }
        
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateGrImportLDto : BaseImportLineUpdateDto
    {
        public long? LineId { get; set; }
        public long? HeaderId { get; set; }
    }
}
