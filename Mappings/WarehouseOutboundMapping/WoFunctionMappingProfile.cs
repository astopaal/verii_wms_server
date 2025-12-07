using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WoFunctionMappingProfile : Profile
    {
        public WoFunctionMappingProfile()
        {
            CreateMap<FN_WoOpenOrder_Header, WoOpenOrderHeaderDto>();
            CreateMap<FN_WoOpenOrder_Line, WoOpenOrderLineDto>();
        }
    }
}
