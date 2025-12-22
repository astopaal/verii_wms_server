using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrHeaderDto : BaseHeaderEntityDto
    {
        [Required]
        [StringLength(30)]
        public string CustomerCode { get; set; } = null!;

        public string? CustomerName { get; set; }

        public bool ReturnCode { get; set; } = false;
        public bool OCRSource { get; set; } = false;

        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description3 { get; set; }
        
        [StringLength(100)]
        public string? Description4 { get; set; }
        
        [StringLength(100)]
        public string? Description5 { get; set; }
    }

    public class CreateGrHeaderDto : BaseHeaderCreateDto
    {
        [Required]
        [StringLength(30)]
        public string CustomerCode { get; set; } = null!;

        public bool ReturnCode { get; set; } = false;
        public bool OCRSource { get; set; } = false;

        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description3 { get; set; }
        
        [StringLength(100)]
        public string? Description4 { get; set; }
        
        [StringLength(100)]
        public string? Description5 { get; set; }
    }

    public class UpdateGrHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(30)]
        public string? CustomerCode { get; set; }

        public bool? ReturnCode { get; set; }
        public bool? OCRSource { get; set; }

        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description3 { get; set; }
        
        [StringLength(100)]
        public string? Description4 { get; set; }
        
        [StringLength(100)]
        public string? Description5 { get; set; }
    }

    public class GrAssignedOrderLinesDto
    {
        public IEnumerable<GrLineDto> Lines { get; set; } = Array.Empty<GrLineDto>();
        public IEnumerable<GrLineSerialDto> LineSerials { get; set; } = Array.Empty<GrLineSerialDto>();
        public IEnumerable<GrImportLDto> ImportLines { get; set; } = Array.Empty<GrImportLDto>();
        public IEnumerable<GrRouteDto> Routes { get; set; } = Array.Empty<GrRouteDto>();
    }
}
