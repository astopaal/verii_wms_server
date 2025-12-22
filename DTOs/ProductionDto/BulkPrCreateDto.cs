using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GenerateProductionOrderRequestDto
    {
        [Required]
        public CreatePrHeaderDto Header { get; set; } = null!;
        public List<CreatePrLineWithKeyDto>? Lines { get; set; }
        public List<CreatePrLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreatePrHeaderSerialDto>? HeaderSerials { get; set; }
        public List<CreatePrTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class CreatePrLineWithKeyDto : CreatePrLineDto
    {
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }
    }

    public class CreatePrLineSerialWithLineKeyDto : CreatePrLineSerialDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreatePrTerminalLineWithUserDto
    {
        public long TerminalUserId { get; set; }
    }
}
