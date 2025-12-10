using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_IMPORT_LINE")]
    public class PrImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual PrHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual PrLine? Line { get; set; }

        public virtual ICollection<PrRoute> Routes { get; set; } = new List<PrRoute>();
    }
}
