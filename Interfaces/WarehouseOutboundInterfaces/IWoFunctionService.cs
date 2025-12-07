using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoFunctionService
    {
        Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode);
        Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv);
    }
}
