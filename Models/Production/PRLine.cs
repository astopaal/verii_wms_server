using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_LINE")]
    public class PrLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual PrHeader Header { get; set; } = null!;

        public virtual ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
        public virtual ICollection<PrLineSerial> SerialLines { get; set; } = new List<PrLineSerial>();
    }
}
