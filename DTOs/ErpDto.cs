using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    // Stok Eldeki Miktar DTO
    public class OnHandQuantityDto
    {
        public int DepoKodu { get; set; }
        public string? StokKodu { get; set; }
        public string? ProjeKodu { get; set; }
        public string? SeriNo { get; set; }
        public string? HucreKodu { get; set; }
        public string? Kaynak { get; set; }
        public decimal? Bakiye { get; set; }
        public string? StokAdi { get; set; }
        public string? DepoIsmi { get; set; }
    }

    // Cari DTO
    public class CariDto
    {
        public short SubeKodu { get; set; }
        public short IsletmeKodu { get; set; }
        public string CariKod { get; set; } = string.Empty;
        public string? CariTel { get; set; }
        public string? CariIl { get; set; }
        public string? UlkeKodu { get; set; }
        public string? CariIsim { get; set; }
        public char? CariTip { get; set; }
        public string? GrupKodu { get; set; }
        public string? RaporKodu1 { get; set; }
        public string? RaporKodu2 { get; set; }
        public string? RaporKodu3 { get; set; }
        public string? RaporKodu4 { get; set; }
        public string? RaporKodu5 { get; set; }
        public string? CariAdres { get; set; }
        public string? CariIlce { get; set; }
        public string? VergiDairesi { get; set; }
        public string? VergiNumarasi { get; set; }
        public string? Fax { get; set; }
        public string? PostaKodu { get; set; }
        public short? DetayKodu { get; set; }
        public decimal? NakliyeKatsayisi { get; set; }
        public decimal? RiskSiniri { get; set; }
        public decimal? Teminati { get; set; }
        public decimal? CariRisk { get; set; }
        public decimal? CcRisk { get; set; }
        public decimal? SaRisk { get; set; }
        public decimal? ScRisk { get; set; }
        public decimal? CmBorct { get; set; }
        public decimal? CmAlact { get; set; }
        public DateTime? CmRapTarih { get; set; }
        public string? KosulKodu { get; set; }
        public decimal? IskontoOrani { get; set; }
        public short? VadeGunu { get; set; }
        public byte? ListeFiati { get; set; }
        public string? Acik1 { get; set; }
        public string? Acik2 { get; set; }
        public string? Acik3 { get; set; }
        public string? MKod { get; set; }
        public byte? DovizTipi { get; set; }
        public byte? DovizTuru { get; set; }
        public char? HesapTutmaSekli { get; set; }
        public char? DovizLimi { get; set; }
    }

    // Stok DTO
    public class StokDto
    {
        public short SubeKodu { get; set; }
        public short IsletmeKodu { get; set; }
        public string StokKodu { get; set; } = string.Empty;
        public string UreticiKodu { get; set; } = string.Empty;
        public string StokAdi { get; set; } = string.Empty;
        public string GrupKodu { get; set; } = string.Empty;
        public string SaticiKodu { get; set; } = string.Empty;
        public string OlcuBr1 { get; set; } = string.Empty;
        public string OlcuBr2 { get; set; } = string.Empty;
        public decimal Pay1 { get; set; }
        public string Kod1 { get; set; } = string.Empty;
        public string Kod2 { get; set; } = string.Empty;
        public string Kod3 { get; set; } = string.Empty;
        public string Kod4 { get; set; } = string.Empty;
        public string Kod5 { get; set; } = string.Empty;
    }

    public class DepoDto
    {
        public short DepoKodu { get; set; }
        public string DepoIsmi { get; set; } = string.Empty;
    }

    public class ProjeDto
    {
        public string ProjeKod { get; set; } = string.Empty;
        public string ProjeAciklama { get; set; } = string.Empty;
    }

    // Müşteri Siparişi DTO
    public class OpenGoodsForOrderByCustomerDto
    {
        public string FatirsNo { get; set; } = string.Empty;
        public DateTime Tarih { get; set; }
        public decimal? BrutTutar { get; set; }
    }

    // Sipariş Detay DTO
    public class OpenGoodsForOrderDetailDto
    {
        public string StokKodu { get; set; } = string.Empty;
        public decimal? KalanMiktar { get; set; }
        public short? DepoKodu { get; set; }
        public string? DepoIsmi { get; set; }
        public string? StokAdi { get; set; }
        public string GirisSeri { get; set; } = string.Empty;
        public string SeriMik { get; set; } = string.Empty;
    }

    // Genel API Response DTO
    public class ErpApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int TotalCount { get; set; }
    }

    // Sayfalama için DTO
    public class ErpPagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    public class ErpPagedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    // Stok Barkod DTO
    public class StokBarcodeDto
    {
        public string? Barkod { get; set; }
        public string? StokKodu { get; set; }
        public string? StokAdi { get; set; }
        public string? DepoKodu { get; set; }
        public string? DepoAdi { get; set; }
        public string? RafKodu { get; set; }
        public char? Yapilandir { get; set; }
        public int? OlcuBr { get; set; }
        public string? OlcuAdi { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public double? Cevrim { get; set; }
        public bool? SeriBarkodMu { get; set; }
        public bool? SktVarmi { get; set; }
        public string? IsemriNo { get; set; }
    }

    public class BranchDto
    {
        public short SubeKodu { get; set; }
        public string? Unvan { get; set; }
    }

    // Stok Yap Kod DTO
    public class StokYapKodDto
    {
        public short IsletmeKodu { get; set; }
        public string YapKod { get; set; } = string.Empty;
        public string YapAcik { get; set; } = string.Empty;
        public string? RevizyapKod { get; set; }
        public string? YplndrStokKod { get; set; }
        public short SubeKodu { get; set; }
    }

    public class WarehouseAndShelvesDto
    {
        [MaxLength(100)]
        public string? DepoKodu { get; set; }

        [MaxLength(100)]
        public string? HucreKodu { get; set; }
    }

    public class WarehouseShelvesWithStockInformationDto
    {
        public string? DepoKodu { get; set; }
        public string? HucreKodu { get; set; }
        public string? StokKodu { get; set; }
        public string? StokAdi { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        
        public string? SeriNo { get; set; }
        public decimal? Bakiye { get; set; }
    }

    public class ProductHeaderDto
    {
        public string? IsemriNo { get; set; }
        public string? StokKodu { get; set; }
        public string? StokAdi { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public decimal? Miktar { get; set; }
        public int? Oncelik { get; set; }
        public string? RefIsemriNo { get; set; }
    }

    public class ProductLineDto
    {
        public string? MamulKodu { get; set; }
        public string? MamulAdi { get; set; }
        public string? IsemriNo { get; set; }
        public string? SiparisNo { get; set; }
        public string? HamKodu { get; set; }
        public string? HamMaddeAdi { get; set; }
        public decimal? BirimMiktar { get; set; }
        public decimal? HesaplananToplamMiktar { get; set; }
        public string? OpNo { get; set; }
    }

    public class ShelfStockDto
    {
        public string? StokKodu { get; set; }
        public string? StokAdi { get; set; }
        public string? YapKod { get; set; }
        public string? YapAcik { get; set; }
        public string? SeriNo {get; set;}
        public decimal? Bakiye { get; set; }
    }

    public class WarehouseShelfStocksDto
    {
        public string? DepoKodu { get; set; }
        public string? HucreKodu { get; set; }
        public List<ShelfStockDto> Stoklar { get; set; } = new();
    }
}
