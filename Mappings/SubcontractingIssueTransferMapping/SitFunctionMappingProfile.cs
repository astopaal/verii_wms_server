using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class SitFunctionMappingProfile : Profile
    {
        public SitFunctionMappingProfile()
        {
            CreateMap<FN_SitOpenOrder_Header, SitOpenOrderHeaderDto>();
            CreateMap<FN_SitOpenOrder_Line, SitOpenOrderLineDto>();
        }
    }
}
