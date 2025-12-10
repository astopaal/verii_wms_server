using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_ROUTE")]
    public class PrRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        [ForeignKey(nameof(ImportLineId))]
        public virtual PrImportLine ImportLine { get; set; } = null!;
    }
}
