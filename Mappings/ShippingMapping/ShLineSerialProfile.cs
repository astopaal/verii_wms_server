using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ShLineSerialProfile : Profile
    {
        public ShLineSerialProfile()
        {
            CreateMap<ShLineSerial, ShLineSerialDto>()
                .ApplyFullUserNames<ShLineSerial, ShLineSerialDto>();

            CreateMap<CreateShLineSerialDto, ShLineSerial>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore());

            CreateMap<UpdateShLineSerialDto, ShLineSerial>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
