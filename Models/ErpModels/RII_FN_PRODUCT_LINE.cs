using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_PRODUCT_LINE
    {
        [MaxLength(35)]
        public string? MAMUL_KODU { get; set; }

        [MaxLength(200)]
        public string? MAMUL_ADI { get; set; }

        [MaxLength(15)]
        public string? ISEMRINO { get; set; }

        [MaxLength(15)]
        public string? SIPARIS_NO { get; set; }

        [MaxLength(35)]
        public string? HAM_KODU { get; set; }

        [MaxLength(200)]
        public string? HAM_MADDE_ADI { get; set; }

        [Column(TypeName = "decimal(28,8)")]
        public decimal? BIRIM_MIKTAR { get; set; }

        [Column(TypeName = "decimal(38,6)")]
        public decimal? HESAPLANAN_TOPLAM_MIKTAR { get; set; }

        [MaxLength(8)]
        public string? OPNO { get; set; }
    }
}
