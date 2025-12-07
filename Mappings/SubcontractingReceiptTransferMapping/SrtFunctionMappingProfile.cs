using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class SrtFunctionMappingProfile : Profile
    {
        public SrtFunctionMappingProfile()
        {
            CreateMap<FN_SrtOpenOrder_Header, SrtOpenOrderHeaderDto>();
            CreateMap<FN_SrtOpenOrder_Line, SrtOpenOrderLineDto>();
        }
    }
}
