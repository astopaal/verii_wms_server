using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class PrImportLineWithRoutesDto
    {
        public PrImportLineDto ImportLine { get; set; } = null!;
        public List<PrRouteDto> Routes { get; set; } = new();
    }
}

