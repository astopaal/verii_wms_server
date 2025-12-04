using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public class FN_TransferOpenOrder_Line
    {
        [Required]
        [MaxLength(1)]
        public string Mode { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string SiparisNo { get; set; } = null!;

        [Required]
        public int OrderID { get; set; }
        public string? StockCode { get; set; }                  // varchar(35), nullable
        public string? StockName { get; set; }                  // varchar(35), nullable
        public string? YapKod { get; set; }                     // varchar(35), nullable
        public string? YapAcik { get; set; }                    // varchar(35), nullable
        [MaxLength(35)]
        public string? CustomerCode { get; set; }

        [MaxLength(100)]
        public string? CustomerName { get; set; }

        [Required]
        public int BranchCode { get; set; }

        public int? TargetWh { get; set; }

        [MaxLength(50)]
        public string? ProjectCode { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? OrderedQty { get; set; }

        public decimal? DeliveredQty { get; set; }

        public decimal? RemainingHamax { get; set; }

        [Required]
        public decimal PlannedQtyAllocated { get; set; }

        public decimal? RemainingForImport { get; set; }
    }
}
