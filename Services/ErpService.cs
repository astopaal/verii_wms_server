using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Services
{
    public class ErpService : IErpService
    {
        private readonly ErpDbContext _erpContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErpService(ErpDbContext erpContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _erpContext = erpContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        // OnHandQuantity işlemleri

        public async Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null)
        {
            try
            {
                var stokParam = string.IsNullOrWhiteSpace(stokKodu) ? null : stokKodu;
                var seriParam = string.IsNullOrWhiteSpace(seriNo) ? null : seriNo;
                var projeParam = string.IsNullOrWhiteSpace(projeKodu) ? null : projeKodu;

                var rows = await _erpContext.OnHandQuantities
                .FromSqlRaw("SELECT * FROM dbo.RII_FN_ONHANDQUANTITY({0}, {1}, {2}, {3})", depoKodu, stokKodu, seriNo, projeKodu)
                .AsNoTracking()
                .ToListAsync();

                var mappedList = _mapper.Map<List<OnHandQuantityDto>>(rows);

                return ApiResponse<List<OnHandQuantityDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("OnHandQuantityRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OnHandQuantityDto>>.ErrorResult(_localizationService.GetLocalizedString("OnHandQuantityRetrievalError"), ex.Message, 500, ex.Message);
            }
        }

        // Cari işlemleri
        public async Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu)
        {
            try
            {
                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

                var result = await _erpContext.Caris
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_CARI({0}, {1})", string.IsNullOrWhiteSpace(cariKodu) ? null : cariKodu, subeKodu)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<CariDto>>(result);
                return ApiResponse<List<CariDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CariDto>>.ErrorResult(_localizationService.GetLocalizedString("CariRetrievalError"), ex.Message, 500, "Error retrieving Cari data");
            }
        }

        public async Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari)
        {
            try
            {
                var codes = (cariKodlari ?? Array.Empty<string>())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .Distinct()
                    .ToList();

                var cariParam = codes.Count == 0 ? null : string.Join(",", codes);

                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeCsv = string.IsNullOrWhiteSpace(subeFromContext)
                    ? null
                    : string.Join(",", subeFromContext.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));

                var result = await _erpContext.Caris
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_CARI({0}, {1})", cariParam, subeCsv)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<CariDto>>(result);
                return ApiResponse<List<CariDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CariDto>>.ErrorResult(_localizationService.GetLocalizedString("CariRetrievalError"), ex.Message, 500, "Error retrieving Cari data");
            }
        }

        // Stok işlemleri
        public async Task<ApiResponse<List<StokDto>>> GetStoksAsync(string? stokKodu)
        {
            try
            {
                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

                var result = await _erpContext.Stoks
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOK({0}, {1})", string.IsNullOrWhiteSpace(stokKodu) ? null : stokKodu, subeKodu)
                    .AsNoTracking()
                    .ToListAsync();
                var mappedResult = _mapper.Map<List<StokDto>>(result);

                return ApiResponse<List<StokDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StokDto>>.ErrorResult(_localizationService.GetLocalizedString("StokRetrievalError"), ex.Message, 500, "Error retrieving Stok data");
            }
        }

        

        

        public async Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos) where T : BaseImportLineEntityDto
        {
            try
            {
                var list = (dtos ?? Array.Empty<T>()).ToList();
                var neededCodes = list
                    .Select(d => d.StockCode)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s!.Trim())
                    .Distinct()
                    .ToList();
                    
                var stokParam = neededCodes.Count == 0 ? null : string.Join(",", neededCodes);
                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeCsv = string.IsNullOrWhiteSpace(subeFromContext)
                    ? null
                    : string.Join(",", subeFromContext.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));

                var result = await _erpContext.Stoks
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOK({0}, {1})", stokParam, subeCsv)
                    .AsNoTracking()
                    .ToListAsync();

                var data = _mapper.Map<List<StokDto>>(result);
                var stockNameByCode = data
                    .Where(s => !string.IsNullOrWhiteSpace(s.StokKodu))
                    .GroupBy(s => s.StokKodu!.Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First().StokAdi ?? string.Empty, StringComparer.OrdinalIgnoreCase);

                foreach (var dto in list)
                {
                    var code = dto.StockCode?.Trim();
                    if (!string.IsNullOrEmpty(code) && stockNameByCode.TryGetValue(code, out var name))
                    {
                        dto.StockName = name;
                    }
                }

                return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<T>>.ErrorResult(_localizationService.GetLocalizedString("StokRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        // Depo işlemleri
        public async Task<ApiResponse<List<DepoDto>>> GetDeposAsync(short? depoKodu)
        {
            try
            {
                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

                var result = await _erpContext.Depos
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_DEPO({0}, {1})", depoKodu, subeKodu)
                    .AsNoTracking()
                    .ToListAsync();
                var mappedResult = _mapper.Map<List<DepoDto>>(result);

                return ApiResponse<List<DepoDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("DepoRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DepoDto>>.ErrorResult(_localizationService.GetLocalizedString("DepoRetrievalError"), ex.Message, 500, "Error retrieving Depo data");
            }
        }

        // Proje işlemleri
        public async Task<ApiResponse<List<ProjeDto>>> GetProjelerAsync()
        {
            try
            {
                var result = await _erpContext.Projeler.ToListAsync();
                var mappedResult = _mapper.Map<List<ProjeDto>>(result);

                return ApiResponse<List<ProjeDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProjeRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProjeDto>>.ErrorResult(_localizationService.GetLocalizedString("ProjeRetrievalError"), ex.Message, 500, "Error retrieving Proje data");
            }
        }
        public async Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru)
        {
            try
            {
                var rows = await _erpContext.StokBarcodes
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOKBARCODE({0}, {1}, {2}, {3}, {4}, {5})", bar, depoKodu, modul, kullaniciId, barkodGrubu, hareketTuru)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedList = _mapper.Map<List<StokBarcodeDto>>(rows);
                return ApiResponse<List<StokBarcodeDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("StokBarcodeRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StokBarcodeDto>>.ErrorResult(_localizationService.GetLocalizedString("StokBarcodeRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null)
        {
            try
            {
                var rows = await _erpContext.Branches
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_BRANCHES({0})", branchNo)
                    .AsNoTracking()
                    .ToListAsync();
                var mappedList = _mapper.Map<List<BranchDto>>(rows);
                return ApiResponse<List<BranchDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("BranchesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BranchDto>>.ErrorResult(_localizationService.GetLocalizedString("BranchesRetrievalError"), ex.Message, 500);
            }
        }

    }
}
