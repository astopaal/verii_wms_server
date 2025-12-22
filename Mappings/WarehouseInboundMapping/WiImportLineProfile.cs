using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WiImportLineProfile : Profile
    {
        public WiImportLineProfile()
        {
            CreateMap<WiImportLine, WiImportLineDto>()
                .ApplyFullUserNames<WiImportLine, WiImportLineDto>();

            CreateMap<CreateWiImportLineDto, WiImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                ;

            CreateMap<UpdateWiImportLineDto, WiImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore())
                ;

            // CreateWiImportLineWithKeysDto to WiImportLine (for BulkCreateWarehouseInboundAsync)
            CreateMap<CreateWiImportLineWithKeysDto, WiImportLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.LineId, opt => opt.Ignore())
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Line, opt => opt.Ignore());
        }
    }
}