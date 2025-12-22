using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtFunctionService
    {
        Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo);
        Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null);
    }
}
