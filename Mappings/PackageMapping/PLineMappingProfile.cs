using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class PLineMappingProfile : Profile
    {
        public PLineMappingProfile()
        {
            CreateMap<PLine, PLineDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PackingHeaderId, opt => opt.MapFrom(src => src.PackingHeaderId))
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YapKod))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceRouteId, opt => opt.MapFrom(src => src.SourceRouteId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ApplyFullUserNames<PLine, PLineDto>();

            CreateMap<CreatePLineDto, PLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeader, opt => opt.Ignore())
                .ForMember(dest => dest.Package, opt => opt.Ignore());

            CreateMap<UpdatePLineDto, PLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeader, opt => opt.Ignore())
                .ForMember(dest => dest.Package, opt => opt.Ignore());
        }
    }
}

