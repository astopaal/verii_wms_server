using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_P_LINE")]
    public class PLine : BaseEntity
    {

        public long PackingHeaderId { get; set; }
        [ForeignKey(nameof(PackingHeaderId))]
        public virtual PHeader PackingHeader { get; set; } = null!;

        public long PackageId { get; set; }
        [ForeignKey(nameof(PackageId))]
        public virtual PPackage Package { get; set; } = null!;

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required, MaxLength(50)]
        public string StockCode { get; set; } = null!;

        [MaxLength(50)]
        public string? YapKod { get; set; }

        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; }

        [MaxLength(50)]
        public string? SerialNo { get; set; }

        [MaxLength(50)]
        public string? SerialNo2 { get; set; }

        [MaxLength(50)]
        public string? SerialNo3 { get; set; }

        [MaxLength(50)]
        public string? SerialNo4 { get; set; }
        
        public long? SourceRouteId { get; set; }
        
    }
}
