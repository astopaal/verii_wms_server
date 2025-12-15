using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PrTerminalLineDto
    {
        public long Id { get; set; }
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreatePrTerminalLineDto : BaseTerminalLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdatePrTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}
