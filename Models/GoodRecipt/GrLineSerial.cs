using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models
{
    [Table("RII_GR_LINE_SERIAL")]
    public class GrLineSerial : BaseLineSerialEntity
    {
        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual GrLine? Line { get; set; }
    }
}
