using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WiFunctionMappingProfile : Profile
    {
        public WiFunctionMappingProfile()
        {
            CreateMap<FN_WiOpenOrder_Header, WiOpenOrderHeaderDto>();
            CreateMap<FN_WiOpenOrder_Line, WiOpenOrderLineDto>();
        }
    }
}
