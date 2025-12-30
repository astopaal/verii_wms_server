using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GenerateGoodReceiptOrderRequestDto
    {
        [Required]
        public CreateGrHeaderDto Header { get; set; } = null!;
        public List<CreateGrLineWithKeyDto>? Lines { get; set; }
        public List<CreateGrLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateGrTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class CreateGrLineSerialWithLineKeyDto : CreateGrLineSerialDto
    {
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateGrTerminalLineWithUserDto : BaseTerminalLineCreateDto
    {
    }
}

