using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Services
{
    public class WoFunctionService : IWoFunctionService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WoFunctionService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_WoOpenOrder_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WO_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<WoOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<WoOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WoOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _wmsDbContext.Set<FN_WoOpenOrder_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WO_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();
                var dtos = _mapper.Map<List<WoOpenOrderLineDto>>(rows);
                return ApiResponse<List<WoOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WoOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
