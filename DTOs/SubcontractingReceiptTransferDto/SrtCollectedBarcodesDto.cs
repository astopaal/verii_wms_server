using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class SrtImportLineWithRoutesDto
    {
        public SrtImportLineDto ImportLine { get; set; } = null!;
        public List<SrtRouteDto> Routes { get; set; } = new();
    }
}

