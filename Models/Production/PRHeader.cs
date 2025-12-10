using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_HEADER")]
    public class PrHeader : BaseHeaderEntity
    {
        [MaxLength(20)]
        public string? CustomerCode { get; set; }

        [MaxLength(20)]
        public string? StockCode { get; set; }

        [MaxLength(20)]
        public string? YapKod { get; set; }

        [MaxLength(20)]
        public string? SourceWarehouse { get; set; }

        [MaxLength(20)]
        public string? TargetWarehouse { get; set; }

        public virtual ICollection<PrLine> Lines { get; set; } = new List<PrLine>();
        public virtual ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
        public virtual ICollection<PrTerminalLine> TerminalLines { get; set; } = new List<PrTerminalLine>();
    }
}
