using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class GrRouteMappingProfile : Profile
    {
        public GrRouteMappingProfile()
        {
            CreateMap<GrRoute, GrRouteDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.ScannedBarcode, opt => opt.MapFrom(src => src.ScannedBarcode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceWarehouse, opt => opt.MapFrom(src => src.SourceWarehouse))
                .ForMember(dest => dest.TargetWarehouse, opt => opt.MapFrom(src => src.TargetWarehouse))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ApplyFullUserNames<GrRoute, GrRouteDto>();

            CreateMap<CreateGrRouteDto, GrRoute>()
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.ScannedBarcode, opt => opt.MapFrom(src => src.ScannedBarcode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceWarehouse, opt => opt.MapFrom(src => src.SourceWarehouse))
                .ForMember(dest => dest.TargetWarehouse, opt => opt.MapFrom(src => src.TargetWarehouse))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<UpdateGrRouteDto, GrRoute>()
                .ForMember(dest => dest.ImportLineId, opt => opt.MapFrom(src => src.ImportLineId))
                .ForMember(dest => dest.LineId, opt => opt.MapFrom(src => src.LineId))
                .ForMember(dest => dest.ScannedBarcode, opt => opt.MapFrom(src => src.ScannedBarcode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity.HasValue ? src.Quantity.Value : 0))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                .ForMember(dest => dest.SerialNo2, opt => opt.MapFrom(src => src.SerialNo2))
                .ForMember(dest => dest.SerialNo3, opt => opt.MapFrom(src => src.SerialNo3))
                .ForMember(dest => dest.SerialNo4, opt => opt.MapFrom(src => src.SerialNo4))
                .ForMember(dest => dest.SourceWarehouse, opt => opt.MapFrom(src => src.SourceWarehouse))
                .ForMember(dest => dest.TargetWarehouse, opt => opt.MapFrom(src => src.TargetWarehouse))
                .ForMember(dest => dest.SourceCellCode, opt => opt.MapFrom(src => src.SourceCellCode))
                .ForMember(dest => dest.TargetCellCode, opt => opt.MapFrom(src => src.TargetCellCode))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }
    }
}
