using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PLineDto : BaseEntityDto
    {
        public long PackingHeaderId { get; set; }
        public long PackageId { get; set; }

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required, MaxLength(50)]
        public string StockCode { get; set; } = null!;

        [MaxLength(200)]
        public string? StockName { get; set; }

        [MaxLength(50)]
        public string? YapKod { get; set; }

        [MaxLength(200)]
        public string? YapAcik { get; set; }

        [Required]
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

    public class CreatePLineDto
    {
        [Required]
        public long PackingHeaderId { get; set; }

        [Required]
        public long PackageId { get; set; }

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required, MaxLength(50)]
        public string StockCode { get; set; } = null!;

        [MaxLength(50)]
        public string? YapKod { get; set; }

        [Required]
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

    public class UpdatePLineDto
    {
        [MaxLength(50)]
        public string? Barcode { get; set; }

        [MaxLength(50)]
        public string? StockCode { get; set; }

        [MaxLength(50)]
        public string? YapKod { get; set; }

        public decimal? Quantity { get; set; }

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

