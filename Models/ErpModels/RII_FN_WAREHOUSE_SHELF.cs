using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_WAREHOUSE_SHELF
    {
        [MaxLength(100)]
        public string? DEPO_KODU { get; set; }

        [MaxLength(100)]
        public string? HUCRE_KODU { get; set; }
    }
}
