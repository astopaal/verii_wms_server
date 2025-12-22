using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ShImportLineProfile : Profile
    {
        public ShImportLineProfile()
        {
            CreateMap<ShImportLine, ShImportLineDto>()
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? ($"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}").Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? ($"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}").Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? ($"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}").Trim() : null));

            CreateMap<CreateShImportLineDto, ShImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForMember(dest => dest.Routes, opt => opt.Ignore());

            CreateMap<UpdateShImportLineDto, ShImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // CreateShImportLineWithKeysDto to ShImportLine (for BulkCreateShipmentAsync)
            CreateMap<CreateShImportLineWithKeysDto, ShImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.LineId, opt => opt.Ignore())
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YapKod))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForMember(dest => dest.Routes, opt => opt.Ignore());
        }
    }
}
