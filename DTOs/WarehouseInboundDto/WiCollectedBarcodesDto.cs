using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class WiImportLineWithRoutesDto
    {
        public WiImportLineDto ImportLine { get; set; } = null!;
        public List<WiRouteDto> Routes { get; set; } = new();
    }
}

