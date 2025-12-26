using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PPackageDto : BaseEntityDto
    {
        public long PackingHeaderId { get; set; }

        [Required, MaxLength(50)]
        public string PackageNo { get; set; } = null!;

        [Required, MaxLength(20)]
        public string PackageType { get; set; } = null!;

        [MaxLength(100)]
        public string? Barcode { get; set; }

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }

        public decimal? NetWeight { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? GrossWeight { get; set; }

        public bool IsMixed { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = null!;
    }

    public class CreatePPackageDto
    {
        [Required]
        public long PackingHeaderId { get; set; }

        [Required, MaxLength(50)]
        public string PackageNo { get; set; } = null!;

        [MaxLength(20)]
        public string? PackageType { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }

        public decimal? NetWeight { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? GrossWeight { get; set; }

        public bool? IsMixed { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }
    }

    public class UpdatePPackageDto
    {
        [MaxLength(50)]
        public string? PackageNo { get; set; }

        [MaxLength(20)]
        public string? PackageType { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }

        public decimal? NetWeight { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? GrossWeight { get; set; }

        public bool? IsMixed { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }
    }
}

