using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Services
{
    public class WiFunctionService : IWiFunctionService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WiFunctionService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<WiOpenOrderHeaderDto>>> GetWiOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_WiOpenOrder_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WI_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<WiOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<WiOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WiOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<WiOpenOrderLineDto>>> GetWiOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_WiOpenOrder_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WI_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<WiOpenOrderLineDto>>(rows);
                return ApiResponse<List<WiOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WiOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
