using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.Models
{
    /// <summary>
    /// Base parameter entity for all service parameters
    /// </summary>
    public abstract class BaseParameter : BaseEntity
    {
        /// <summary>
        /// Allow less quantity based on order (Emre istinaden az miktar)
        /// </summary>
        public bool AllowLessQuantityBasedOnOrder { get; set; } = false;

        /// <summary>
        /// Allow more quantity based on order (Emre istinaden fazla miktar)
        /// </summary>
        public bool AllowMoreQuantityBasedOnOrder { get; set; } = false;

        /// <summary>
        /// Require approval before ERP integration (ERP öncesi onay gerekli mi)
        /// </summary>
        public bool RequireApprovalBeforeErp { get; set; } = false;

        /// <summary>
        /// Require all order items to be collected (Emirdeki tüm kalemlere toplama yapılmış olmalı)
        /// </summary>
        public bool RequireAllOrderItemsCollected { get; set; } = false;
    }
}

