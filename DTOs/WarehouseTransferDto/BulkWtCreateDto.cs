using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    // TR Line için istemci korelasyon anahtarı
    public class CreateWtLineWithKeyDto : BaseLineCreateDto
    {
        [StringLength(100)]
        public string? ClientKey { get; set; }
        public Guid? ClientGuid { get; set; }

        public int? OrderId { get; set; }
        public string? ErpLineReference { get; set; }
    }

    

    // Tek depo transferi oluşturma isteği
    public class CreateWtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
    {
        [StringLength(100)]
        public string? LineClientKey { get; set; }
        public Guid? LineGroupGuid { get; set; }
    }

    public class CreateWtTerminalLineWithUserDto : BaseTerminalLineCreateDto
    {
    }

      public class BulkWtHeaderCreateDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public CreateWtHeaderDto Header { get; set; } = null!;
    }

    public class CreateWtLineWithKeysDto : BaseLineCreateDto
    {
        [Required]
        public Guid HeaderKey { get; set; }
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreateWtLineSerialWithGuidDto : BaseLineSerialCreateDto
    {
        [Required]
        public Guid LineKey { get; set; }
    }

    public class CreateWtImportLineWithGuidDto : BaseImportLineCreateDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }
        [Required]
        public Guid HeaderKey { get; set; }
        public Guid? LineKey { get; set; }
    }

    public class CreateWtRouteWithImportGuidDto : BaseRouteCreateDto
    {
        [Required]
        public Guid ImportLineKey { get; set; }

        public string? Description { get; set; }
    }

    public class GenerateWarehouseTransferOrderRequestDto
    {
        [Required]
        public CreateWtHeaderDto Header { get; set; } = null!;
        public List<CreateWtLineWithKeyDto>? Lines { get; set; }
        public List<CreateWtLineSerialWithLineKeyDto>? LineSerials { get; set; }
        public List<CreateWtTerminalLineWithUserDto>? TerminalLines { get; set; }
    }

    public class BulkWtGenerateRequestDto
    {
        [Required]
        public BulkWtHeaderCreateDto Header { get; set; } = null!;
        public List<CreateWtLineWithKeysDto>? Lines { get; set; }
        public List<CreateWtLineSerialWithGuidDto>? LineSerials { get; set; }
        public List<CreateWtImportLineWithGuidDto>? ImportLines { get; set; }
        public List<CreateWtRouteWithImportGuidDto>? Routes { get; set; }
    }
}
