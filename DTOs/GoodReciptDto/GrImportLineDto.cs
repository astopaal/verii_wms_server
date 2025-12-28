using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrImportLineDto : BaseImportLineEntityDto
    {
        public long? LineId { get; set; }
        public long HeaderId { get; set; }
    }

    public class GrImportLineWithRoutesDto
    {
        public GrImportLineDto ImportLine { get; set; } = null!;
        public List<GrRouteDto> Routes { get; set; } = new();
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

