using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class SitTerminalLineProfile : Profile
    {
        public SitTerminalLineProfile()
        {
            CreateMap<SitTerminalLine, SitTerminalLineDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.TerminalUserId, opt => opt.MapFrom(src => src.TerminalUserId))
                .ApplyFullUserNames<SitTerminalLine, SitTerminalLineDto>();

            CreateMap<CreateSitTerminalLineDto, SitTerminalLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.TerminalUserId, opt => opt.MapFrom(src => src.TerminalUserId))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateSitTerminalLineDto, SitTerminalLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.TerminalUserId, opt => opt.MapFrom(src => src.TerminalUserId))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // CreateSitTerminalLineWithUserDto to SitTerminalLine (for GenerateOrderAsync)
            CreateMap<CreateSitTerminalLineWithUserDto, SitTerminalLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.TerminalUserId, opt => opt.MapFrom(src => src.TerminalUserId))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());
        }
    }
}