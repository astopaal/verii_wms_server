using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    public class RII_FN_BRANCHES
    {
        public short SUBE_KODU { get; set; }

        [MaxLength(150)]
        public string? UNVAN { get; set; }
    }
}
