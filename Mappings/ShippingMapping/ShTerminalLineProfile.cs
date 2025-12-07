using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ShTerminalLineProfile : Profile
    {
        public ShTerminalLineProfile()
        {
            CreateMap<ShTerminalLine, ShTerminalLineDto>()
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? ($"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}").Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? ($"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}").Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? ($"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}").Trim() : null));

            CreateMap<CreateShTerminalLineDto, ShTerminalLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateShTerminalLineDto, ShTerminalLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
