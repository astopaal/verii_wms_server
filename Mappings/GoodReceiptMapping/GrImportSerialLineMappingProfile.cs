using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class GrImportSerialLineMappingProfile : Profile
    {
        public GrImportSerialLineMappingProfile()
        {
            // GrImportSerialLine mappings
            CreateMap<GrLineSerial, GrLineSerialDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ApplyFullUserNames<GrLineSerial, GrLineSerialDto>();

            CreateMap<CreateGrLineSerialDto, GrLineSerial>()
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLine, opt => opt.Ignore());

            CreateMap<UpdateGrLineSerialDto, GrLineSerial>()
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLine, opt => opt.Ignore());

            // CreateGrImportSerialLineWithImportLineKeyDto to GrLineSerial (for BulkCreateAsync)
            CreateMap<CreateGrImportSerialLineWithImportLineKeyDto, GrLineSerial>()
                .ForMember(dest => dest.ImportLineId, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLine, opt => opt.Ignore())
                .ForMember(dest => dest.ClientKey, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLineClientKey, opt => opt.Ignore());
        }
    }
}
