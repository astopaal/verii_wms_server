using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_HEADER")]
    public class ShHeader : BaseHeaderEntity
    {
        [MaxLength(20)]
        public string? CustomerCode { get; set; }

        [MaxLength(20)]
        public string? SourceWarehouse { get; set; }

        [MaxLength(20)]
        public string? TargetWarehouse { get; set; }

        public long? ShipmentId { get; set; }

        public virtual ICollection<ShLine> Lines { get; set; } = new List<ShLine>();
        public virtual ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
        public virtual ICollection<ShTerminalLine> TerminalLines { get; set; } = new List<ShTerminalLine>();
    }
}
