using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services
{
    public class PtFunctionService : IPtFunctionService
    {
        private readonly ErpDbContext _erpContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PtFunctionService(ErpDbContext erpContext, IMapper mapper, ILocalizationService localizationService)
        {
            _erpContext = erpContext;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo)
        {
            try
            {
                var result = await _erpContext.ProductHeaders
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_PRODUCT_HEADER({0})", isemriNo)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<ProductHeaderDto>>(result);

                return ApiResponse<List<ProductHeaderDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ProductHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null)
        {
            try
            {
                var result = await _erpContext.ProductLines
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_PRODUCT_LINE({0}, {1}, {2})", isemriNo, fisNo, mamulKodu)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<ProductLineDto>>(result);

                return ApiResponse<List<ProductLineDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ProductLinesRetrievalError"), ex.Message, 500);
            }
        }
    }
}
