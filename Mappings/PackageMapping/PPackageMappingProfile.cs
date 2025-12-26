using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class PPackageMappingProfile : Profile
    {
        public PPackageMappingProfile()
        {
            CreateMap<PPackage, PPackageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PackingHeaderId, opt => opt.MapFrom(src => src.PackingHeaderId))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.PackageNo))
                .ForMember(dest => dest.PackageType, opt => opt.MapFrom(src => src.PackageType))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
                .ForMember(dest => dest.NetWeight, opt => opt.MapFrom(src => src.NetWeight))
                .ForMember(dest => dest.TareWeight, opt => opt.MapFrom(src => src.TareWeight))
                .ForMember(dest => dest.GrossWeight, opt => opt.MapFrom(src => src.GrossWeight))
                .ForMember(dest => dest.IsMixed, opt => opt.MapFrom(src => src.IsMixed))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ApplyFullUserNames<PPackage, PPackageDto>();

            CreateMap<CreatePPackageDto, PPackage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeader, opt => opt.Ignore())
                .ForMember(dest => dest.Lines, opt => opt.Ignore());

            CreateMap<UpdatePPackageDto, PPackage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PackingHeader, opt => opt.Ignore())
                .ForMember(dest => dest.Lines, opt => opt.Ignore());
        }
    }
}

