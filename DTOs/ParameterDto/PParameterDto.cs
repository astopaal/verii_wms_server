namespace WMS_WEBAPI.DTOs
{
    public class PParameterDto : BaseEntityDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; }
        public bool AllowMoreQuantityBasedOnOrder { get; set; }
        public bool RequireApprovalBeforeErp { get; set; }
    }

    public class CreatePParameterDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; } = false;
        public bool AllowMoreQuantityBasedOnOrder { get; set; } = false;
        public bool RequireApprovalBeforeErp { get; set; } = false;
    }

    public class UpdatePParameterDto
    {
        public bool? AllowLessQuantityBasedOnOrder { get; set; }
        public bool? AllowMoreQuantityBasedOnOrder { get; set; }
        public bool? RequireApprovalBeforeErp { get; set; }
    }
}

