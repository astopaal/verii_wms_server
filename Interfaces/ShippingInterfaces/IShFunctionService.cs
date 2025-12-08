using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShFunctionService
    {
        Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetShippingOpenOrderHeaderAsync(string customerCode);
        Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetShippingOpenOrderLineAsync(string siparisNoCsv);
        
    }
}
