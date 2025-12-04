using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class FN_GoodsOpenOrders_Line
    {
        public string Mode { get; set; } = null!;                 // varchar(1), not null
        public string SiparisNo { get; set; } = null!;            // TDBBELGENO(15), not null
        public int OrderID { get; set; }                          // TDBINTEGER(4), not null
        public string? StockCode { get; set; }                  // varchar(35), nullable
        public string? StockName { get; set; }                  // varchar(35), nullable
        public string? YapKod { get; set; }                       // varchar(50), nullable
        public string? YapAcik { get; set; }                      // varchar(100), nullable
        public string? CustomerCode { get; set; }                 // varchar(35), nullable
        public string? CustomerName { get; set; }                 // varchar(100), nullable
        public int BranchCode { get; set; }                     // TDBSMALLINT(2), not null
        public int? TargetWh { get; set; }                      // TDBSMALLINT(2), nullable
        public string? ProjectCode { get; set; }                  // varchar(50), nullable
        public DateTime? OrderDate { get; set; }                  // TDBDATETIME(8), nullable
        public decimal? OrderedQty { get; set; }                  // decimal(17), nullable
        public decimal? DeliveredQty { get; set; }                // decimal(17), nullable
        public decimal? RemainingHamax { get; set; }              // decimal(9), nullable
        public decimal PlannedQtyAllocated { get; set; }          // decimal(17), not null
        public decimal? RemainingForImport { get; set; }          // decimal(9), nullable
    }
}
