using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WiLineProfile : Profile
    {
        public WiLineProfile()
        {
            CreateMap<WiLine, WiLineDto>()
                .ApplyFullUserNames<WiLine, WiLineDto>();

            CreateMap<CreateWiLineDto, WiLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLines, opt => opt.Ignore())
                .ForMember(dest => dest.SerialLines, opt => opt.Ignore());

            CreateMap<UpdateWiLineDto, WiLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLines, opt => opt.Ignore())
                .ForMember(dest => dest.SerialLines, opt => opt.Ignore());

            // CreateWiLineWithKeyDto to WiLine (for GenerateWarehouseInboundOrderAsync)
            CreateMap<CreateWiLineWithKeyDto, WiLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HeaderId, opt => opt.Ignore())
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.StockCode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ErpOrderNo, opt => opt.MapFrom(src => src.ErpOrderNo))
                .ForMember(dest => dest.ErpOrderId, opt => opt.MapFrom(src => src.ErpOrderId))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.ImportLines, opt => opt.Ignore())
                .ForMember(dest => dest.SerialLines, opt => opt.Ignore())
                .ForMember(dest => dest.ClientKey, opt => opt.Ignore())
                .ForMember(dest => dest.ClientGuid, opt => opt.Ignore());
        }
    }
}