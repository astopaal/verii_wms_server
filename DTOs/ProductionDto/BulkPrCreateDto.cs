using System;
using System.Collections.Generic;
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

    // Bulk Generate için DTO'lar (Guid key'ler ile)
    public class BulkPrHeaderCreateDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public CreatePrHeaderDto Header { get; set; } = null!;
    }

    public class CreatePrLineWithKeysDto : CreatePrLineDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreatePrLineSerialWithGuidDto : CreatePrLineSerialDto
    {
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreatePrHeaderSerialWithGuidDto : CreatePrHeaderSerialDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
    }

    public class CreatePrTerminalLineWithGuidDto : CreatePrTerminalLineWithUserDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
    }

    public class CreatePrImportLineWithGuidDto : CreatePrImportLineDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }
        [Required]
        public Guid HeaderKey { get; set; }
        public Guid? LineKey { get; set; }
    }

    public class CreatePrRouteWithImportGuidDto : CreatePrRouteDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }
    }

    public class BulkPrGenerateRequestDto
    {
        [Required]
        public BulkPrHeaderCreateDto Header { get; set; } = null!;
        public List<CreatePrLineWithKeysDto>? Lines { get; set; }
        public List<CreatePrLineSerialWithGuidDto>? LineSerials { get; set; }
        public List<CreatePrHeaderSerialWithGuidDto>? HeaderSerials { get; set; }
        public List<CreatePrTerminalLineWithGuidDto>? TerminalLines { get; set; }
        public List<CreatePrImportLineWithGuidDto>? ImportLines { get; set; }
        public List<CreatePrRouteWithImportGuidDto>? Routes { get; set; }
    }
}
