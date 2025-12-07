using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Services
{
    public class ShFunctionService : IShFunctionService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShFunctionService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetShippingOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_ShOpenOrder_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_SH_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<TransferOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetShippingOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_ShOpenOrder_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_SH_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<TransferOpenOrderLineDto>>(rows);
                return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
