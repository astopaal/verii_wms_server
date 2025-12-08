using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IErpService
    {
        // OnHandQuantity işlemleri
        Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null);

        // Cari işlemleri
        Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu);
        Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari);

        // Stok işlemleri
        Task<ApiResponse<List<StokDto>>> GetStoksAsync(string? stokKodu);
        Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos) where T : BaseImportLineEntityDto;

        // Depo işlemleri
        Task<ApiResponse<List<DepoDto>>> GetDeposAsync(short? depoKodu);

        // Proje işlemleri
        Task<ApiResponse<List<ProjeDto>>> GetProjelerAsync();

        // Stok barkod işlemleri
        Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru);

        // Şube işlemleri
        Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null);

    }
}
