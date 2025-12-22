using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class GrImportLMappingProfile : Profile
    {
        public GrImportLMappingProfile()
        {
            CreateMap<GrImportLine, GrImportLDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ApplyFullUserNames<GrImportLine, GrImportLDto>();

            CreateMap<GrImportLine, GrImportLWithRoutesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => src.Routes.Where(r => !r.IsDeleted)))
                .ApplyFullUserNames<GrImportLine, GrImportLWithRoutesDto>();

            CreateMap<CreateGrImportLDto, GrImportLine>()
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore());

            CreateMap<UpdateGrImportLDto, GrImportLine>()
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore());

            // CreateGrImportLWithLineKeyDto to GrImportLine (for BulkCreateAsync)
            CreateMap<CreateGrImportLWithLineKeyDto, GrImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.LineId, opt => opt.Ignore())
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore());
        }
    }
}
