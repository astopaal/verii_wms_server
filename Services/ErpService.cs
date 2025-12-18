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

        

        

        public async Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos)
        {
            try
            {
                
                var codeProp = typeof(T).GetProperty("StockCode");
                var nameProp = typeof(T).GetProperty("StockName");
                var yapCodeProp = typeof(T).GetProperty("YapKod");
                var yapNameProp = typeof(T).GetProperty("YapAcik");

                var list = (dtos ?? Array.Empty<T>()).ToList();
                
                var codes = codeProp == null
                    ? new List<string>()
                    : list
                        .Select(d => codeProp.GetValue(d) as string)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s!.Trim())
                        .Distinct()
                        .ToList();

                var stokParam = codes.Count == 0 ? null : string.Join(",", codes);

                var yapKodlar = yapCodeProp == null
                    ? new List<string>()
                    : list
                        .Select(d => yapCodeProp.GetValue(d) as string)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s!.Trim())
                        .Distinct()
                        .ToList();
                var yapParam = yapKodlar.Count == 0 ? null : string.Join(",", yapKodlar);

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

                var resultYapkod = await _erpContext.StokYapKod
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOKYAPKOD({0}, {1})", yapParam, subeCsv)
                    .AsNoTracking()
                    .ToListAsync();

                var dataYapKod = _mapper.Map<List<StokYapKodDto>>(resultYapkod);
                
                var yapNameByCode = dataYapKod
                    .Where(c => !string.IsNullOrWhiteSpace(c.YapKod))
                    .GroupBy(c => c.YapKod.Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First().YapAcik ?? string.Empty, StringComparer.OrdinalIgnoreCase);

                foreach (var dto in list)
                {
                    //Stok koduna göre adını doldur
                    if (codeProp != null && nameProp != null)
                    {
                        var code = codeProp.GetValue(dto) as string;
                        var trimmed = code?.Trim();
                        if (!string.IsNullOrEmpty(trimmed) && stockNameByCode.TryGetValue(trimmed, out var nm))
                        {
                            nameProp.SetValue(dto, nm);
                        }
                    }
                    //Yapılandırma Koduna göre adını doldur
                    if (yapCodeProp != null && yapNameProp != null)
                    {
                        var yapCode = yapCodeProp.GetValue(dto) as string;
                        var trimmedYap = yapCode?.Trim();
                        if (!string.IsNullOrEmpty(trimmedYap) && yapNameByCode.TryGetValue(trimmedYap, out var yapNm))
                        {
                            yapNameProp.SetValue(dto, yapNm);
                        }
                    }
                }

                return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<T>>.ErrorResult(_localizationService.GetLocalizedString("StokRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> dtos)
        {
            try
            {
                var list = (dtos ?? Array.Empty<T>()).ToList();
                var codeProp = typeof(T).GetProperty("CustomerCode");
                var nameProp = typeof(T).GetProperty("CustomerName");

                var codes = codeProp == null
                    ? new List<string>()
                    : list
                        .Select(d => codeProp.GetValue(d) as string)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s!.Trim())
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

                var data = _mapper.Map<List<CariDto>>(result);
                var nameByCode = data
                    .Where(c => !string.IsNullOrWhiteSpace(c.CariKod))
                    .GroupBy(c => c.CariKod.Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First().CariIsim ?? string.Empty, StringComparer.OrdinalIgnoreCase);

 
                foreach (var dto in list)
                {      
                     if (codeProp != null && nameProp != null)
                      {
                        var code = codeProp.GetValue(dto) as string;
                        var trimmed = code?.Trim();
                        if (!string.IsNullOrEmpty(trimmed) && nameByCode.TryGetValue(trimmed, out var nm))
                        {
                            nameProp.SetValue(dto, nm);
                        }
                    }
                }

                return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<T>>.ErrorResult(_localizationService.GetLocalizedString("CariRetrievalError"), ex.Message ?? string.Empty, 500);
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

        public async Task<ApiResponse<IEnumerable<T>>> PopulateWarehouseNamesAsync<T>(IEnumerable<T> dtos)
        {
            try
            {
                var list = (dtos ?? Array.Empty<T>()).ToList();
                var srcCodeProp = typeof(T).GetProperty("SourceWarehouse");
                var srcNameProp = typeof(T).GetProperty("SourceWarehouseName");
                var tgtCodeProp = typeof(T).GetProperty("TargetWarehouse");
                var tgtNameProp = typeof(T).GetProperty("TargetWarehouseName");

                var subeFromContext = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

                var rows = await _erpContext.Depos
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_DEPO({0}, {1})", null, subeKodu)
                    .AsNoTracking()
                    .ToListAsync();

                var depos = _mapper.Map<List<DepoDto>>(rows);
                var nameByCode = depos
                    .GroupBy(d => d.DepoKodu.ToString().Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First().DepoIsmi ?? string.Empty, StringComparer.OrdinalIgnoreCase);

                foreach (var dto in list)
                {
                    // SourceWarehouse/SourceWarehouseName
                    if (srcCodeProp != null && srcNameProp != null)
                    {
                        var codeObj = srcCodeProp.GetValue(dto);
                        var codeStr = codeObj == null ? null : codeObj.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(codeStr) && nameByCode.TryGetValue(codeStr!, out var nm))
                        {
                            srcNameProp.SetValue(dto, nm);
                        }
                    }

                    // TargetWarehouse/TargetWarehouseName
                    if (tgtCodeProp != null && tgtNameProp != null)
                    {
                        var codeObj = tgtCodeProp.GetValue(dto);
                        var codeStr = codeObj == null ? null : codeObj.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(codeStr) && nameByCode.TryGetValue(codeStr!, out var nm))
                        {
                            tgtNameProp.SetValue(dto, nm);
                        }
                    }
                }

                return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("DepoRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<T>>.ErrorResult(_localizationService.GetLocalizedString("DepoRetrievalError"), ex.Message ?? string.Empty, 500);
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

        // Depo ve Raf işlemleri
        public async Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(string? depoKodu = null, string? raf = null)
        {
            try
            {
                var result = await _erpContext.WarehouseAndShelves
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WAREHOUSE_SHELF({0}, {1})", depoKodu, raf)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<WarehouseAndShelvesDto>>(result);

                return ApiResponse<List<WarehouseAndShelvesDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("WarehouseAndShelvesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WarehouseAndShelvesDto>>.ErrorResult(_localizationService.GetLocalizedString("WarehouseAndShelvesRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(string? depoKodu = null, string? raf = null)
        {
            try
            {
                var result = await _erpContext.StockWarehouses
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE({0}, {1})", depoKodu, raf)
                    .AsNoTracking()
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<WarehouseShelvesWithStockInformationDto>>(result);

                return ApiResponse<List<WarehouseShelvesWithStockInformationDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("StockWarehouseRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WarehouseShelvesWithStockInformationDto>>.ErrorResult(_localizationService.GetLocalizedString("StockWarehouseRetrievalError"), ex.Message, 500);
            }
        }

        // Üretim Emri işlemleri
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

        public async Task<ApiResponse<List<WarehouseShelfStocksDto>>> GetWarehouseShelvesNestedAsync(string depoKodu)
        {
            try
            {
                var shelvesRows = await _erpContext.WarehouseAndShelves
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_WAREHOUSE_SHELF({0}, {1})", depoKodu, null)
                    .AsNoTracking()
                    .ToListAsync();
                var shelves = _mapper.Map<List<WarehouseAndShelvesDto>>(shelvesRows);

                var stocksRows = await _erpContext.StockWarehouses
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE({0}, {1})", depoKodu, null)
                    .AsNoTracking()
                    .ToListAsync();

                var stocksByShelf = stocksRows
                    .Where(r => !string.IsNullOrWhiteSpace(r.HUCRE_KODU))
                    .GroupBy(r => r.HUCRE_KODU!.Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => _mapper.Map<List<ShelfStockDto>>(g.ToList()), StringComparer.OrdinalIgnoreCase);

                var result = shelves
                    .OrderBy(s => s.HucreKodu)
                    .Select(s => new WarehouseShelfStocksDto
                    {
                        DepoKodu = s.DepoKodu,
                        HucreKodu = s.HucreKodu,
                        Stoklar = s.HucreKodu != null && stocksByShelf.TryGetValue(s.HucreKodu.Trim(), out var list) ? list : new List<ShelfStockDto>()
                    })
                    .ToList();

                return ApiResponse<List<WarehouseShelfStocksDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WarehouseShelvesNestedRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WarehouseShelfStocksDto>>.ErrorResult(_localizationService.GetLocalizedString("WarehouseShelvesNestedRetrievalError"), ex.Message, 500);
            }
        }

    }
}
