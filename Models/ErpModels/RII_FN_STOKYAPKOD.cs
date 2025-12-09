using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_STOKYAPKOD
    {
        public short ISLETME_KODU { get; set; }

        [MaxLength(15)]
        public string YAPKOD { get; set; } = string.Empty;

        [MaxLength(255)]
        public string YAPACIK { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? REVIZYAPKOD { get; set; }

        [MaxLength(35)]
        public string? YPLNDRSTOKKOD { get; set; }

        public short SUBE_KODU { get; set; }
    }
}
