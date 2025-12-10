using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class PrImportLineProfile : Profile
    {
        public PrImportLineProfile()
        {
            CreateMap<PrImportLine, PrImportLineDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ApplyFullUserNames<PrImportLine, PrImportLineDto>();

            CreateMap<CreatePrImportLineDto, PrImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore());

            CreateMap<UpdatePrImportLineDto, PrImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Description1, opt => opt.MapFrom(src => src.Description1))
                .ForMember(dest => dest.Description2, opt => opt.MapFrom(src => src.Description2))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
