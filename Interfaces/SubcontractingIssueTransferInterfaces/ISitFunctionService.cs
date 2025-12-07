using System.Collections.Generic;
using System.Threading.Tasks;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitFunctionService
    {
        Task<ApiResponse<List<SitOpenOrderHeaderDto>>> GetSitOpenOrderHeaderAsync(string customerCode);
        Task<ApiResponse<List<SitOpenOrderLineDto>>> GetSitOpenOrderLineAsync(string siparisNoCsv);
    }
}
