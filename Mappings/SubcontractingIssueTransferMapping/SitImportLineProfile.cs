using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class SitImportLineProfile : Profile
    {
        public SitImportLineProfile()
        {
            CreateMap<SitImportLine, SitImportLineDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ApplyFullUserNames<SitImportLine, SitImportLineDto>();

            CreateMap<CreateSitImportLineDto, SitImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                ;

            CreateMap<UpdateSitImportLineDto, SitImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // CreateSitImportLineWithKeysDto to SitImportLine (for BulkCreateSubcontractingIssueTransferAsync)
            CreateMap<CreateSitImportLineWithKeysDto, SitImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.LineId, opt => opt.Ignore())
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore());
        }
    }
}