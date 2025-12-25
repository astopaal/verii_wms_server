namespace WMS_WEBAPI.DTOs
{
    public class WiParameterDto : BaseEntityDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; }
        public bool AllowMoreQuantityBasedOnOrder { get; set; }
        public bool RequireApprovalBeforeErp { get; set; }
        public bool RequireAllOrderItemsCollected { get; set; }
    }

    public class CreateWiParameterDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; } = false;
        public bool AllowMoreQuantityBasedOnOrder { get; set; } = false;
        public bool RequireApprovalBeforeErp { get; set; } = false;
        public bool RequireAllOrderItemsCollected { get; set; } = false;
    }

    public class UpdateWiParameterDto
    {
        public bool? AllowLessQuantityBasedOnOrder { get; set; }
        public bool? AllowMoreQuantityBasedOnOrder { get; set; }
        public bool? RequireApprovalBeforeErp { get; set; }
        public bool? RequireAllOrderItemsCollected { get; set; }
    }
}

