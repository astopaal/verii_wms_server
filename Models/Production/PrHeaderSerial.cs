using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PR_HEADER_SERIAL")]
    public class PrHeaderSerial : BaseEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual PrHeader Header { get; set; } = null!;

        [MaxLength(50)]
        public string? SerialNo { get; set; }

        [MaxLength(50)]
        public string? SerialNo2 { get; set; }

        [MaxLength(50)]
        public string? SerialNo3 { get; set; }

        [MaxLength(50)]
        public string? SerialNo4 { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? Amount { get; set; }
    }
}
