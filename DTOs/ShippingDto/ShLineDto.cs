using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
        public string? ErpLineReference { get; set; }
    }

    public class CreateShLineDto : BaseLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
        [StringLength(10)]
        public string? ErpLineReference { get; set; }
    }

    public class UpdateShLineDto : BaseLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public int? OrderId { get; set; }
        [StringLength(10)]
        public string? ErpLineReference { get; set; }
    }
}
