using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_IMPORT_LINE")]
    public class ShImportLine : BaseImportLineEntity
    {
        [Required, ForeignKey(nameof(Header))]
        public long HeaderId { get; set; }
        public virtual ShHeader Header { get; set; } = null!;

        [ForeignKey(nameof(Line))]
        public long? LineId { get; set; }
        public virtual ShLine Line { get; set; } = null!;

        public virtual ICollection<ShRoute> Routes { get; set; } = new List<ShRoute>();
    }
}
