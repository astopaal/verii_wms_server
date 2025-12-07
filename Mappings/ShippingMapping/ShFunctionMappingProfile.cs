using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ShFunctionMappingProfile : Profile
    {
        public ShFunctionMappingProfile()
        {
            CreateMap<FN_ShOpenOrder_Header, TransferOpenOrderHeaderDto>();
            CreateMap<FN_ShOpenOrder_Line, TransferOpenOrderLineDto>();
        }
    }
}
