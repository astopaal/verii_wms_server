using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_LINE_SERIAL")]
    public class PrLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual PrLine Line { get; set; } = null!;
    }
}
