using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class ShHeaderDto
    {
        public long Id { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string? ProjectCode { get; set; }
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
        public string YearCode { get; set; } = string.Empty;
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public byte Type { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsPendingApproval { get; set; } = false;
        public bool? ApprovalStatus { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsERPIntegrated { get; set; } = false;
        public string? ERPReferenceNumber { get; set; }
        public DateTime? ERPIntegrationDate { get; set; }
        public string? ERPIntegrationStatus { get; set; }
        public string? ERPErrorMessage { get; set; }
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreateShHeaderDto : BaseHeaderCreateDto
    {
        [StringLength(50)]
        public string DocumentNo { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        [StringLength(20)]
        public string? CustomerCode { get; set; }
        [StringLength(100)]
        public string? CustomerName { get; set; }
        [StringLength(20)]
        public string? SourceWarehouse { get; set; }
        [StringLength(20)]
        public string? TargetWarehouse { get; set; }
        [Required]
        public byte Type { get; set; }
    }

    public class UpdateShHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(50)]
        public string? DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        [StringLength(20)]
        public string? CustomerCode { get; set; }
        [StringLength(100)]
        public string? CustomerName { get; set; }
        [StringLength(20)]
        public string? SourceWarehouse { get; set; }
        [StringLength(20)]
        public string? TargetWarehouse { get; set; }
        public byte? Type { get; set; }
    }
}
