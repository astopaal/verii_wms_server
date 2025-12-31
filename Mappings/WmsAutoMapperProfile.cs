using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WmsAutoMapperProfile : Profile
    {
        public WmsAutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
            
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // UserDetail mappings
            CreateMap<UserDetail, UserDetailDto>()
                .ApplyFullUserNames();
            
            CreateMap<CreateUserDetailDto, UserDetail>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateUserDetailDto, UserDetail>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public static class MappingExtensions
    {
        private static string? FullName(User? user)
        {
            return user != null ? ($"{user.FirstName} {user.LastName}").Trim() : null;
        }

        public static IMappingExpression<TSource, TDestination> ApplyFullUserNames<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
            where TSource : BaseEntity
            where TDestination : BaseEntityDto
        {
            expr.ForMember(nameof(BaseEntityDto.CreatedByFullUser), opt => opt.MapFrom(src => FullName(src.CreatedByUser)));
            expr.ForMember(nameof(BaseEntityDto.UpdatedByFullUser), opt => opt.MapFrom(src => FullName(src.UpdatedByUser)));
            expr.ForMember(nameof(BaseEntityDto.DeletedByFullUser), opt => opt.MapFrom(src => FullName(src.DeletedByUser)));
            return expr;
        }
    }
}