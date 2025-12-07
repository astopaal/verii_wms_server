using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_ROUTE")]
    public class ShRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        [ForeignKey(nameof(ImportLineId))]
        public virtual ShImportLine ImportLine { get; set; } = null!;
    }
}
