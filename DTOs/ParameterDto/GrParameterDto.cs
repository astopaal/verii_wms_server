namespace WMS_WEBAPI.DTOs
{
    public class GrParameterDto : BaseEntityDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; }
        public bool AllowMoreQuantityBasedOnOrder { get; set; }
        public bool RequireApprovalBeforeErp { get; set; }
    }

    public class CreateGrParameterDto
    {
        public bool AllowLessQuantityBasedOnOrder { get; set; } = false;
        public bool AllowMoreQuantityBasedOnOrder { get; set; } = false;
        public bool RequireApprovalBeforeErp { get; set; } = false;
    }

    public class UpdateGrParameterDto
    {
        public bool? AllowLessQuantityBasedOnOrder { get; set; }
        public bool? AllowMoreQuantityBasedOnOrder { get; set; }
        public bool? RequireApprovalBeforeErp { get; set; }
    }
}

