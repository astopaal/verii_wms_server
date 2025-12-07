using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SH_TERMINAL_LINE")]
    public class ShTerminalLine : BaseEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual ShHeader Header { get; set; } = null!;

        public long TerminalUserId { get; set; }
        [ForeignKey(nameof(TerminalUserId))]
        public virtual User User { get; set; } = null!;
    }
}
