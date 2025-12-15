using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseRouteEntity : BaseEntity
    {
        [MaxLength(75)]
        public string ScannedBarcode { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        // Seri bilgileri (isteğe bağlı, kalabilir veya kaldırılabilir)
        [MaxLength(50)]
        public string? SerialNo { get; set; }
        
        [MaxLength(50)]
        public string? SerialNo2 { get; set; }

        [MaxLength(50)]
        public string? SerialNo3 { get; set; }

        [MaxLength(50)]
        public string? SerialNo4 { get; set; }

        // Location details
        public int? SourceWarehouse { get; set; }
        public int? TargetWarehouse { get; set; }

        [MaxLength(20)]
        public string? SourceCellCode { get; set; }

        [MaxLength(20)]
        public string? TargetCellCode { get; set; }

    }
}
