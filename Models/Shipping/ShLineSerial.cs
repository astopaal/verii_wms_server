using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_LINE_SERIAL")]
    public class ShLineSerial : BaseLineSerialEntity
    {
        [Required, ForeignKey(nameof(Line))]
        public long LineId { get; set; }
        public virtual ShLine Line { get; set; } = null!;
    }
}
