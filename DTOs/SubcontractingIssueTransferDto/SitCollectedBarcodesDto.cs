using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class SitImportLineWithRoutesDto
    {
        public SitImportLineDto ImportLine { get; set; } = null!;
        public List<SitRouteDto> Routes { get; set; } = new();
    }
}

