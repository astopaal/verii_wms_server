using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_STOKBARCODE
    {
        [MaxLength(100)]
        public string? BARKOD { get; set; }

        [MaxLength(100)]
        public string? STOK_KODU { get; set; }

        [MaxLength(50)]
        public string? STOK_ADI { get; set; }

        [MaxLength(50)]
        public string? DEPO_KODU { get; set; }

        [MaxLength(50)]
        public string? DEPO_ADI { get; set; }

        [MaxLength(50)]
        public string? RAF_KODU { get; set; }

        public char? YAPILANDIR { get; set; }

        public int? OLCU_BR { get; set; }

        [MaxLength(2)]
        public string? OLCU_ADI { get; set; }

        [MaxLength(15)]
        public string? YAPKOD { get; set; }

        [MaxLength(50)]
        public string? YAPACIK { get; set; }

        public double? CEVRIM { get; set; }

        public bool? SERI_BARKODUMU { get; set; }

        public bool? SKT_VARMI { get; set; }

        [MaxLength(15)]
        public string? ISEMRI_NO { get; set; }
    }
}
