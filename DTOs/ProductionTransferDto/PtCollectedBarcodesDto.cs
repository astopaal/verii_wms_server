using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class PtImportLineWithRoutesDto
    {
        public PtImportLineDto ImportLine { get; set; } = null!;
        public List<PtRouteDto> Routes { get; set; } = new();
    }
}

