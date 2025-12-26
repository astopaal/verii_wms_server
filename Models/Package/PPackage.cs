using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_P_PACKAGE")]
    public class PPackage : BaseEntity
    {
        // PackingHeader Foreign Key
        public long PackingHeaderId { get; set; }
        [ForeignKey(nameof(PackingHeaderId))]
        public virtual PHeader PackingHeader { get; set; } = null!;

        // Paket numarası (Koli-1, Palet-2)
        [Required, MaxLength(50)]
        public string PackageNo { get; set; } = null!;

        // Paket tipi: Box / Pallet / Bag / Custom
        [Required, MaxLength(20)]
        public string PackageType { get; set; } = PPackageType.Box;

        // Koli barkodu (SSCC uyumlu olabilir)
        [MaxLength(100)]
        public string? Barcode { get; set; }

        // Boyut bilgileri
        [Column(TypeName = "decimal(18,6)")]
        public decimal? Length { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? Width { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? Volume { get; set; }

        // Ağırlık bilgileri
        [Column(TypeName = "decimal(18,6)")]
        public decimal? NetWeight { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TareWeight { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? GrossWeight { get; set; }

        // Farklı ürün içeriyor mu
        public bool IsMixed { get; set; } = false;

        // Durum: Open / Closed / Loaded
        [Required, MaxLength(20)]
        public string Status { get; set; } = PPackageStatus.Open;

        // Navigasyon özellikleri
        public virtual ICollection<PLine> Lines { get; set; } = new List<PLine>();
    }
}
