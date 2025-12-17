using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_PRODUCT_HEADER
    {
        [MaxLength(15)]
        public string? ISEMRINO { get; set; }

        [MaxLength(35)]
        public string? STOK_KODU { get; set; }

        [MaxLength(200)]
        public string? STOK_ADI { get; set; }

        [MaxLength(1)]
        public string? YAPKOD { get; set; }

        [MaxLength(1)]
        public string? YAPACIK { get; set; }

        [Column(TypeName = "decimal(28,8)")]
        public decimal? MIKTAR { get; set; }

        public int? ONCELIK { get; set; }

        [MaxLength(15)]
        public string? REFISEMRINO { get; set; }
    }
}
