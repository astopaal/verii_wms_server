using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtHeaderDto : BaseHeaderEntityDto
    {
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouse { get; set; }
        public string? TargetWarehouseName { get; set; }
    }

    public class CreatePtHeaderDto : BaseHeaderCreateDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }

    public class UpdatePtHeaderDto : BaseHeaderUpdateDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }

    public class PtAssignedProductionTransferOrderLinesDto
    {
        public IEnumerable<PtLineDto> Lines { get; set; } = Array.Empty<PtLineDto>();
        public IEnumerable<PtLineSerialDto> LineSerials { get; set; } = Array.Empty<PtLineSerialDto>();
        public IEnumerable<PtImportLineDto> ImportLines { get; set; } = Array.Empty<PtImportLineDto>();
        public IEnumerable<PtRouteDto> Routes { get; set; } = Array.Empty<PtRouteDto>();
    }
}