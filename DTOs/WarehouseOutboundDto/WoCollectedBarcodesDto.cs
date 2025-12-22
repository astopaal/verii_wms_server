using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class WoImportLineWithRoutesDto
    {
        public WoImportLineDto ImportLine { get; set; } = null!;
        public List<WoRouteDto> Routes { get; set; } = new();
    }
}

