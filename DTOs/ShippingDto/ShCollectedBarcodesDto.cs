using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class ShImportLineWithRoutesDto
    {
        public ShImportLineDto ImportLine { get; set; } = null!;
        public List<ShRouteDto> Routes { get; set; } = new();
    }
}

