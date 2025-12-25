namespace WMS_WEBAPI.DTOs
{
    public class ShParameterDto : BaseEntityDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; }
        public bool AllowMoreQuantityBasedOnOrder { get; set; }
        public bool RequireApprovalBeforeErp { get; set; }
        public bool RequireAllOrderItemsCollected { get; set; }
    }

    public class CreateShParameterDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; } = false;
        public bool AllowMoreQuantityBasedOnOrder { get; set; } = false;
        public bool RequireApprovalBeforeErp { get; set; } = false;
        public bool RequireAllOrderItemsCollected { get; set; } = false;
    }

    public class UpdateShParameterDto
    {
        public bool? AllowLessQuantityBasedOnOrder { get; set; }
        public bool? AllowMoreQuantityBasedOnOrder { get; set; }
        public bool? RequireApprovalBeforeErp { get; set; }
        public bool? RequireAllOrderItemsCollected { get; set; }
    }
}

