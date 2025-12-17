using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IErpService
    {
        // OnHandQuantity işlemleri
        Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null);

        // Cari işlemleri
        Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu);
        Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> dtos);

        // Stok işlemleri
        Task<ApiResponse<List<StokDto>>> GetStoksAsync(string? stokKodu);
        Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos);

        // Depo işlemleri
        Task<ApiResponse<List<DepoDto>>> GetDeposAsync(short? depoKodu);
        Task<ApiResponse<IEnumerable<T>>> PopulateWarehouseNamesAsync<T>(IEnumerable<T> dtos);

        // Proje işlemleri
        Task<ApiResponse<List<ProjeDto>>> GetProjelerAsync();

        // Stok barkod işlemleri
        Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru);

        // Şube işlemleri
        Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null);

        // Depo ve Raf işlemleri
        Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(string? depoKodu = null, string? raf = null);
        Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(string? depoKodu = null, string? raf = null);

        // Üretim Emri işlemleri
        Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo);
        Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null);

    }
}
