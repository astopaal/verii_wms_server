using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Services
{
    public class WtFunctionService : IWtFunctionService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WtFunctionService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetTransferOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var headers = await _wmsDbContext.Set<FN_TransferOpenOrder_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WT_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();

                var headerDtos = _mapper.Map<List<TransferOpenOrderHeaderDto>>(headers);

                return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(
                    headerDtos,
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderHeaderRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderHeaderRetrievalError"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }

        public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetTransferOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var lines = await _wmsDbContext.Set<FN_TransferOpenOrder_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();

                var lineDtos = _mapper.Map<List<TransferOpenOrderLineDto>>(lines);

                return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(
                    lineDtos,
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderLineRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderLineRetrievalError"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }
    }
}