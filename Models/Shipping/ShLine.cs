using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_LINE")]
    public class ShLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual ShHeader Header { get; set; } = null!;

        public virtual ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
        public virtual ICollection<ShLineSerial> SerialLines { get; set; } = new List<ShLineSerial>();
    }
}
