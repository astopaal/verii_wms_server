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
        }
    }
}
