using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class IcImportLineWithRoutesDto
    {
        public IcImportLineDto ImportLine { get; set; } = null!;
        public List<IcRouteDto> Routes { get; set; } = new();
    }
}

