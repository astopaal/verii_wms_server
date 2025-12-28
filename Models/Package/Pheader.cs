using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_P_HEADER")]
    public class PHeader : BaseEntity
    {

        public string? WarehouseCode { get; set; }

        // Paket numarası (PKG-2025-000123 formatında)
        [Required, MaxLength(50)]
        public string PackingNo { get; set; } = null!;

        // Paketleme tarihi
        public DateTime? PackingDate { get; set; }

        // Kaynak tipi: SalesOrder / Transfer / Production / Return
        [MaxLength(30)]
        public string? SourceType { get; set; }

        // Kaynak Header ID (İlgili belgenin header Id'si)
        public long? SourceHeaderId { get; set; }

        // Müşteri bilgileri
        [MaxLength(50)]
        public string? CustomerCode { get; set; }
        [MaxLength(255)]
        public string? CustomerAddress { get; set; }

        // Durum: Draft / Packing / Packed / Shipped / Cancelled
        [Required, MaxLength(20)]
        public string Status { get; set; } = PHeaderStatus.Draft;

        // Toplam bilgileri
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalPackageCount { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalQuantity { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalNetWeight { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalGrossWeight { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? TotalVolume { get; set; }

        // Kargo bilgileri
        public long? CarrierId { get; set; }
        [MaxLength(20)]
        public string? CarrierServiceType { get; set; }
        [MaxLength(100)]
        public string? TrackingNo { get; set; }

        // Eşleşme durumu
        public bool IsMatched { get; set; } = false;

        // Navigasyon özellikleri
        public virtual ICollection<PPackage> Packages { get; set; } = new List<PPackage>();
    }
}

