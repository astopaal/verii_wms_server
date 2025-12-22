using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrHeaderSerialDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public decimal? Amount { get; set; }
    }

    public class CreatePrHeaderSerialDto
    {
        [Required]
        public long HeaderId { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public decimal? Amount { get; set; }
    }

    public class UpdatePrHeaderSerialDto
    {
        [Required]
        public long Id { get; set; }
        public long? HeaderId { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public decimal? Amount { get; set; }
    }
}
