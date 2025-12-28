using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class PHeaderMappingProfile : Profile
    {
        public PHeaderMappingProfile()
        {
            CreateMap<PHeader, PHeaderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom(src => src.WarehouseCode))
                .ForMember(dest => dest.PackingNo, opt => opt.MapFrom(src => src.PackingNo))
                .ForMember(dest => dest.PackingDate, opt => opt.MapFrom(src => src.PackingDate))
                .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => src.SourceType))
                .ForMember(dest => dest.SourceHeaderId, opt => opt.MapFrom(src => src.SourceHeaderId))
                .ForMember(dest => dest.CustomerCode, opt => opt.MapFrom(src => src.CustomerCode))
                .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.CustomerAddress))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalPackageCount, opt => opt.MapFrom(src => src.TotalPackageCount))
                .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.TotalQuantity))
                .ForMember(dest => dest.TotalNetWeight, opt => opt.MapFrom(src => src.TotalNetWeight))
                .ForMember(dest => dest.TotalGrossWeight, opt => opt.MapFrom(src => src.TotalGrossWeight))
                .ForMember(dest => dest.TotalVolume, opt => opt.MapFrom(src => src.TotalVolume))
                .ForMember(dest => dest.CarrierId, opt => opt.MapFrom(src => src.CarrierId))
                .ForMember(dest => dest.CarrierServiceType, opt => opt.MapFrom(src => src.CarrierServiceType))
                .ForMember(dest => dest.TrackingNo, opt => opt.MapFrom(src => src.TrackingNo))
                .ForMember(dest => dest.IsMatched, opt => opt.MapFrom(src => src.IsMatched))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ApplyFullUserNames<PHeader, PHeaderDto>();

            CreateMap<CreatePHeaderDto, PHeader>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsMatched, opt => opt.MapFrom(src => src.IsMatched ?? false))
                .ForMember(dest => dest.Packages, opt => opt.Ignore());

            CreateMap<UpdatePHeaderDto, PHeader>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsMatched, opt => opt.MapFrom(src => src.IsMatched ?? false))
                .ForMember(dest => dest.Packages, opt => opt.Ignore());
        }
    }
}

