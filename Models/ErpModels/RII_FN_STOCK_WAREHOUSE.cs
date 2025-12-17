using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_STOCK_WAREHOUSE
    {
        [MaxLength(100)]
        public string? DEPO_KODU { get; set; }

        [MaxLength(100)]
        public string? HUCRE_KODU { get; set; }

        [MaxLength(100)]
        public string? STOK_KODU { get; set; }

        [MaxLength(100)]
        public string? STOK_ADI { get; set; }

        [MaxLength(100)]
        public string? YAPKOD { get; set; }

        [MaxLength(100)]
        public string? YAPACIK { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? BAKIYE { get; set; }
    }
}
