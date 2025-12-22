using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ShRouteProfile : Profile
    {
        public ShRouteProfile()
        {
            CreateMap<ShRoute, ShRouteDto>()
                .ApplyFullUserNames<ShRoute, ShRouteDto>();

            CreateMap<CreateShRouteDto, ShRoute>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLine, opt => opt.Ignore());

            CreateMap<UpdateShRouteDto, ShRoute>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // CreateShRouteWithLineKeyDto to ShRoute (for BulkCreateShipmentAsync)
            CreateMap<CreateShRouteWithLineKeyDto, ShRoute>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLineId, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.ScannedBarcode, opt => opt.MapFrom(src => src.ScannedBarcode))
                .ForMember(dest => dest.SourceWarehouse, opt => opt.MapFrom(src => src.SourceWarehouse))
                .ForMember(dest => dest.TargetWarehouse, opt => opt.MapFrom(src => src.TargetWarehouse))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLine, opt => opt.Ignore());
        }
    }
}
