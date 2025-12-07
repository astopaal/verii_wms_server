using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    public class FN_SitOpenOrder_Header
    {
        [Required]
        [MaxLength(1)]
        public string Mode { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string SiparisNo { get; set; } = null!;

        public int? OrderID { get; set; }
        [MaxLength(30)]
        public string? CustomerCode { get; set; }
        [MaxLength(100)]
        public string? CustomerName { get; set; }
        public short? BranchCode { get; set; }
        public short? TargetWh { get; set; }
        [MaxLength(50)]
        public string? ProjectCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderedQty { get; set; }
        public decimal? DeliveredQty { get; set; }
        public decimal? RemainingHamax { get; set; }
        public decimal PlannedQtyAllocated { get; set; }
        public decimal? RemainingForImport { get; set; }
    }
}
