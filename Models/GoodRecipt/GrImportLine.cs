using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models
{
    [Table("RII_GR_IMPORT_LINE")]
    public class GrImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual GrHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual GrLine? Line { get; set; }
        
        //navigation properties
        public virtual ICollection<GrRoute> Routes { get; set; } = new List<GrRoute>();
    }
}
