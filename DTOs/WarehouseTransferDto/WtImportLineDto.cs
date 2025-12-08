using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WtImportLineDto : BaseImportLineEntityDto
    {
        public long Id { get; set; }
        public long LineId { get; set; }
        public long RouteId { get; set; }
        public string? Unit { get; set; }
        public string? CellCode { get; set; }
        public string? CellName { get; set; }
        public string? CellType { get; set; }
        public string? CellLocation { get; set; }
        public string? CellCapacity { get; set; }
        public string? CellStatus { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductSpecifications { get; set; }
        public string? BatchNo { get; set; }
        public string? LotNo { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public string? Dimensions { get; set; }
        public string? QualityStatus { get; set; }
        public string? QualityNotes { get; set; }
        public string? DamageStatus { get; set; }
        public string? DamageNotes { get; set; }
        public string? HandlingInstructions { get; set; }
        public string? SpecialRequirements { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public string? EnvironmentalConditions { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? ProcessedQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string? CompletedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        
        public string? Notes { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        
        // Full user information properties
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreateWtImportLineDto
    {
        [Required]
        public long LineId { get; set; }

        [Required]
        public long RouteId { get; set; }

        [Required]
        [StringLength(50)]
        public string StockCode { get; set; } = string.Empty;

        [StringLength(200)]
        public string? StockName { get; set; }

        

        [StringLength(10)]
        public string? Unit { get; set; }

        [StringLength(20)]
        public string? CellCode { get; set; }

        [StringLength(100)]
        public string? CellName { get; set; }

        [StringLength(20)]
        public string? CellType { get; set; }

        [StringLength(50)]
        public string? CellLocation { get; set; }

        [StringLength(20)]
        public string? CellCapacity { get; set; }

        [StringLength(20)]
        public string? CellStatus { get; set; }

        [StringLength(50)]
        public string? ProductCode { get; set; }

        [StringLength(200)]
        public string? ProductName { get; set; }

        [StringLength(50)]
        public string? ProductType { get; set; }

        [StringLength(50)]
        public string? ProductCategory { get; set; }

        [StringLength(500)]
        public string? ProductDescription { get; set; }

        [StringLength(500)]
        public string? ProductSpecifications { get; set; }

        [StringLength(50)]
        public string? BatchNo { get; set; }

        [StringLength(50)]
        public string? LotNo { get; set; }

        

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }

        [StringLength(50)]
        public string? Dimensions { get; set; }

        [StringLength(20)]
        public string? QualityStatus { get; set; }

        [StringLength(500)]
        public string? QualityNotes { get; set; }

        [StringLength(20)]
        public string? DamageStatus { get; set; }

        [StringLength(500)]
        public string? DamageNotes { get; set; }

        [StringLength(500)]
        public string? HandlingInstructions { get; set; }

        [StringLength(500)]
        public string? SpecialRequirements { get; set; }

        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }

        [StringLength(200)]
        public string? EnvironmentalConditions { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public decimal? ProcessedQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }

        [StringLength(50)]
        public string? AssignedTo { get; set; }

        public DateTime? AssignedDate { get; set; }

        [StringLength(50)]
        public string? CompletedBy { get; set; }

        public DateTime? CompletedDate { get; set; }

        

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateWtImportLineDto
    {
        public long? LineId { get; set; }
        public long? RouteId { get; set; }

        [StringLength(50)]
        public string? StockCode { get; set; }

        [StringLength(200)]
        public string? StockName { get; set; }

        

        [StringLength(10)]
        public string? Unit { get; set; }

        [StringLength(20)]
        public string? CellCode { get; set; }

        [StringLength(100)]
        public string? CellName { get; set; }

        [StringLength(20)]
        public string? CellType { get; set; }

        [StringLength(50)]
        public string? CellLocation { get; set; }

        [StringLength(20)]
        public string? CellCapacity { get; set; }

        [StringLength(20)]
        public string? CellStatus { get; set; }

        [StringLength(50)]
        public string? ProductCode { get; set; }

        [StringLength(200)]
        public string? ProductName { get; set; }

        [StringLength(50)]
        public string? ProductType { get; set; }

        [StringLength(50)]
        public string? ProductCategory { get; set; }

        [StringLength(500)]
        public string? ProductDescription { get; set; }

        [StringLength(500)]
        public string? ProductSpecifications { get; set; }

        [StringLength(50)]
        public string? BatchNo { get; set; }

        [StringLength(50)]
        public string? LotNo { get; set; }

        [StringLength(50)]
        public string? SerialNo { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }

        [StringLength(50)]
        public string? Dimensions { get; set; }

        [StringLength(20)]
        public string? QualityStatus { get; set; }

        [StringLength(500)]
        public string? QualityNotes { get; set; }

        [StringLength(20)]
        public string? DamageStatus { get; set; }

        [StringLength(500)]
        public string? DamageNotes { get; set; }

        [StringLength(500)]
        public string? HandlingInstructions { get; set; }

        [StringLength(500)]
        public string? SpecialRequirements { get; set; }

        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }

        [StringLength(200)]
        public string? EnvironmentalConditions { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        public decimal? ProcessedQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }

        [StringLength(50)]
        public string? AssignedTo { get; set; }

        public DateTime? AssignedDate { get; set; }

        [StringLength(50)]
        public string? CompletedBy { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(50)]
        public string? ErpOrderNo { get; set; }

        public int? ErpLineNo { get; set; }

        [StringLength(20)]
        public string? ErpStatus { get; set; }

        public DateTime? ErpSyncDate { get; set; }

        [StringLength(500)]
        public string? ErpDescription { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
