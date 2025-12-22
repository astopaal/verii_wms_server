using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    // Tek production transfer order oluşturma için DTO'lar
    public class CreatePtLineWithKeyDto : CreatePtLineDto
    {
        [StringLength(100)]
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }
    }

    public class CreatePtLineSerialWithLineKeyDto : CreatePtLineSerialDto
    {
        [StringLength(100)]
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreatePtTerminalLineWithUserDto : BaseTerminalLineCreateDto
    {
    }

    // Bulk Generate için DTO'lar (Guid key'ler ile)
    public class BulkPtHeaderCreateDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public CreatePtHeaderDto Header { get; set; } = null!;
    }

    public class CreatePtLineWithKeysDto : CreatePtLineDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreatePtLineSerialWithGuidDto : CreatePtLineSerialDto
    {
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreatePtImportLineWithGuidDto : CreatePtImportLineDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }
        [Required]
        public Guid HeaderKey { get; set; }
        public Guid? LineKey { get; set; }
    }

    public class CreatePtRouteWithImportGuidDto : CreatePtRouteDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }
    }

    // Request DTO'ları
    public class GenerateProductionTransferOrderRequestDto
    {
        [Required]
        public CreatePtHeaderDto Header { get; set; } = null!;
        public List<CreatePtLineWithKeyDto>? Lines { get; set; }
        public List<CreatePtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreatePtTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class BulkPtGenerateRequestDto
    {
        [Required]
        public BulkPtHeaderCreateDto Header { get; set; } = null!;
        public List<CreatePtLineWithKeysDto>? Lines { get; set; }
        public List<CreatePtLineSerialWithGuidDto>? LineSerials { get; set; }
        public List<CreatePtImportLineWithGuidDto>? ImportLines { get; set; }
        public List<CreatePtRouteWithImportGuidDto>? Routes { get; set; }
    }
}

