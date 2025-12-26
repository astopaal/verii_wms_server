using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PHeaderDto : BaseEntityDto
    {
        [MaxLength(20)]
        public string? WarehouseCode { get; set; }

        [Required, MaxLength(50)]
        public string PackingNo { get; set; } = null!;

        public DateTime? PackingDate { get; set; }

        [MaxLength(30)]
        public string? SourceType { get; set; }

        public long? SourceHeaderId { get; set; }

        [MaxLength(50)]
        public string? CustomerCode { get; set; }

        [MaxLength(200)]
        public string? CustomerName { get; set; }

        [MaxLength(255)]
        public string? CustomerAddress { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = null!;

        public decimal? TotalPackageCount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalNetWeight { get; set; }
        public decimal? TotalGrossWeight { get; set; }
        public decimal? TotalVolume { get; set; }

        public long? CarrierId { get; set; }

        [MaxLength(20)]
        public string? CarrierServiceType { get; set; }

        [MaxLength(100)]
        public string? TrackingNo { get; set; }
    }

    public class CreatePHeaderDto
    {
        [MaxLength(20)]
        public string? WarehouseCode { get; set; }

        [Required, MaxLength(50)]
        public string PackingNo { get; set; } = null!;

        public DateTime? PackingDate { get; set; }

        [MaxLength(30)]
        public string? SourceType { get; set; }

        public long? SourceHeaderId { get; set; }

        [MaxLength(50)]
        public string? CustomerCode { get; set; }

        [MaxLength(255)]
        public string? CustomerAddress { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        public decimal? TotalPackageCount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalNetWeight { get; set; }
        public decimal? TotalGrossWeight { get; set; }
        public decimal? TotalVolume { get; set; }

        public long? CarrierId { get; set; }

        [MaxLength(20)]
        public string? CarrierServiceType { get; set; }

        [MaxLength(100)]
        public string? TrackingNo { get; set; }
    }

    public class UpdatePHeaderDto
    {
        [MaxLength(20)]
        public string? WarehouseCode { get; set; }

        [MaxLength(50)]
        public string? PackingNo { get; set; }

        public DateTime? PackingDate { get; set; }

        [MaxLength(30)]
        public string? SourceType { get; set; }

        public long? SourceHeaderId { get; set; }

        [MaxLength(50)]
        public string? CustomerCode { get; set; }

        [MaxLength(255)]
        public string? CustomerAddress { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        public decimal? TotalPackageCount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalNetWeight { get; set; }
        public decimal? TotalGrossWeight { get; set; }
        public decimal? TotalVolume { get; set; }

        public long? CarrierId { get; set; }

        [MaxLength(20)]
        public string? CarrierServiceType { get; set; }

        [MaxLength(100)]
        public string? TrackingNo { get; set; }
    }
}

