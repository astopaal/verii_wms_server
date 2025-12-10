using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
    }

    public class CreatePrLineDto : BaseLineCreateDto
    {
        public long HeaderId { get; set; }
    }

    public class UpdatePrLineDto : BaseLineUpdateDto
    {
        public long? HeaderId { get; set; }
    }
}
