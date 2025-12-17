using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class ErpMappingProfile : Profile
    {
        public ErpMappingProfile()
        {
            // ERP Model mappings
            CreateMap<RII_FN_ONHANDQUANTITY, OnHandQuantityDto>()
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.ProjeKodu, opt => opt.MapFrom(src => src.PROJE_KODU))
                .ForMember(dest => dest.SeriNo, opt => opt.MapFrom(src => src.SERI_NO))
                .ForMember(dest => dest.HucreKodu, opt => opt.MapFrom(src => src.HUCRE_KODU))
                .ForMember(dest => dest.Kaynak, opt => opt.MapFrom(src => src.KAYNAK))
                .ForMember(dest => dest.Bakiye, opt => opt.MapFrom(src => src.BAKIYE))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.DepoIsmi, opt => opt.MapFrom(src => src.DEPO_ISMI));

            CreateMap<RII_VW_CARI, CariDto>()
                .ForMember(dest => dest.SubeKodu, opt => opt.MapFrom(src => src.SUBE_KODU))
                .ForMember(dest => dest.IsletmeKodu, opt => opt.MapFrom(src => src.ISLETME_KODU))
                .ForMember(dest => dest.CariKod, opt => opt.MapFrom(src => src.CARI_KOD))
                .ForMember(dest => dest.CariTel, opt => opt.MapFrom(src => src.CARI_TEL))
                .ForMember(dest => dest.CariIl, opt => opt.MapFrom(src => src.CARI_IL))
                .ForMember(dest => dest.UlkeKodu, opt => opt.MapFrom(src => src.ULKE_KODU))
                .ForMember(dest => dest.CariIsim, opt => opt.MapFrom(src => src.CARI_ISIM))
                .ForMember(dest => dest.CariTip, opt => opt.MapFrom(src => src.CARI_TIP))
                .ForMember(dest => dest.GrupKodu, opt => opt.MapFrom(src => src.GRUP_KODU));

            CreateMap<RII_VW_STOK, StokDto>()
                .ForMember(dest => dest.SubeKodu, opt => opt.MapFrom(src => src.SUBE_KODU))
                .ForMember(dest => dest.IsletmeKodu, opt => opt.MapFrom(src => src.ISLETME_KODU))
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.UreticiKodu, opt => opt.MapFrom(src => src.URETICI_KODU))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.GrupKodu, opt => opt.MapFrom(src => src.GRUP_KODU))
                .ForMember(dest => dest.SaticiKodu, opt => opt.MapFrom(src => src.SATICI_KODU))
                .ForMember(dest => dest.OlcuBr1, opt => opt.MapFrom(src => src.OLCU_BR1))
                .ForMember(dest => dest.OlcuBr2, opt => opt.MapFrom(src => src.OLCU_BR2))
                .ForMember(dest => dest.Pay1, opt => opt.MapFrom(src => src.PAY_1))
                .ForMember(dest => dest.Kod1, opt => opt.MapFrom(src => src.KOD_1))
                .ForMember(dest => dest.Kod2, opt => opt.MapFrom(src => src.KOD_2))
                .ForMember(dest => dest.Kod3, opt => opt.MapFrom(src => src.KOD_3))
                .ForMember(dest => dest.Kod4, opt => opt.MapFrom(src => src.KOD_4))
                .ForMember(dest => dest.Kod5, opt => opt.MapFrom(src => src.KOD_5));

            CreateMap<RII_VW_DEPO, DepoDto>()
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.DepoIsmi, opt => opt.MapFrom(src => src.DEPO_ISMI));

            CreateMap<RII_VW_PROJE, ProjeDto>()
                .ForMember(dest => dest.ProjeKod, opt => opt.MapFrom(src => src.ProjeKod))
                .ForMember(dest => dest.ProjeAciklama, opt => opt.MapFrom(src => src.ProjeAciklama));

            CreateMap<RII_FN_OPENGOODSFORORDERBYCUSTOMER, OpenGoodsForOrderByCustomerDto>()
                .ForMember(dest => dest.FatirsNo, opt => opt.MapFrom(src => src.FATIRS_NO))
                .ForMember(dest => dest.Tarih, opt => opt.MapFrom(src => src.TARIH))
                .ForMember(dest => dest.BrutTutar, opt => opt.MapFrom(src => src.BRUTTUTAR));

            CreateMap<RII_FN_OPENGOODSFORORDERDETAILBYORDERNUMBERS, OpenGoodsForOrderDetailDto>()
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.KalanMiktar, opt => opt.MapFrom(src => src.KALAN_MIKTAR))
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.DepoIsmi, opt => opt.MapFrom(src => src.DEPO_ISMI))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.GirisSeri, opt => opt.MapFrom(src => src.GIRIS_SERI))
                .ForMember(dest => dest.SeriMik, opt => opt.MapFrom(src => src.SERI_MIK));

            CreateMap<RII_FN_STOKBARCODE, StokBarcodeDto>()
                .ForMember(dest => dest.Barkod, opt => opt.MapFrom(src => src.BARKOD))
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.DepoAdi, opt => opt.MapFrom(src => src.DEPO_ADI))
                .ForMember(dest => dest.RafKodu, opt => opt.MapFrom(src => src.RAF_KODU))
                .ForMember(dest => dest.Yapilandir, opt => opt.MapFrom(src => src.YAPILANDIR))
                .ForMember(dest => dest.OlcuBr, opt => opt.MapFrom(src => src.OLCU_BR))
                .ForMember(dest => dest.OlcuAdi, opt => opt.MapFrom(src => src.OLCU_ADI))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YAPKOD))
                .ForMember(dest => dest.YapAcik, opt => opt.MapFrom(src => src.YAPACIK))
                .ForMember(dest => dest.Cevrim, opt => opt.MapFrom(src => src.CEVRIM))
                .ForMember(dest => dest.SeriBarkodMu, opt => opt.MapFrom(src => src.SERI_BARKODUMU))
                .ForMember(dest => dest.SktVarmi, opt => opt.MapFrom(src => src.SKT_VARMI))
                .ForMember(dest => dest.IsemriNo, opt => opt.MapFrom(src => src.ISEMRI_NO));

            CreateMap<RII_FN_STOKYAPKOD, StokYapKodDto>()
                .ForMember(dest => dest.IsletmeKodu, opt => opt.MapFrom(src => src.ISLETME_KODU))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YAPKOD))
                .ForMember(dest => dest.YapAcik, opt => opt.MapFrom(src => src.YAPACIK))
                .ForMember(dest => dest.RevizyapKod, opt => opt.MapFrom(src => src.REVIZYAPKOD))
                .ForMember(dest => dest.YplndrStokKod, opt => opt.MapFrom(src => src.YPLNDRSTOKKOD))
                .ForMember(dest => dest.SubeKodu, opt => opt.MapFrom(src => src.SUBE_KODU));

            CreateMap<RII_FN_BRANCHES, BranchDto>()
                .ForMember(dest => dest.SubeKodu, opt => opt.MapFrom(src => src.SUBE_KODU))
                .ForMember(dest => dest.Unvan, opt => opt.MapFrom(src => src.UNVAN));

            CreateMap<RII_FN_WAREHOUSE_SHELF, WarehouseAndShelvesDto>()
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.HucreKodu, opt => opt.MapFrom(src => src.HUCRE_KODU));

            CreateMap<RII_FN_STOCK_WAREHOUSE, WarehouseShelvesWithStockInformationDto>()
                .ForMember(dest => dest.DepoKodu, opt => opt.MapFrom(src => src.DEPO_KODU))
                .ForMember(dest => dest.HucreKodu, opt => opt.MapFrom(src => src.HUCRE_KODU))
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YAPKOD))
                .ForMember(dest => dest.YapAcik, opt => opt.MapFrom(src => src.YAPACIK))
                .ForMember(dest => dest.Bakiye, opt => opt.MapFrom(src => src.BAKIYE));

            CreateMap<RII_FN_PRODUCT_HEADER, ProductHeaderDto>()
                .ForMember(dest => dest.IsemriNo, opt => opt.MapFrom(src => src.ISEMRINO))
                .ForMember(dest => dest.StokKodu, opt => opt.MapFrom(src => src.STOK_KODU))
                .ForMember(dest => dest.StokAdi, opt => opt.MapFrom(src => src.STOK_ADI))
                .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.YAPKOD))
                .ForMember(dest => dest.YapAcik, opt => opt.MapFrom(src => src.YAPACIK))
                .ForMember(dest => dest.Miktar, opt => opt.MapFrom(src => src.MIKTAR))
                .ForMember(dest => dest.Oncelik, opt => opt.MapFrom(src => src.ONCELIK))
                .ForMember(dest => dest.RefIsemriNo, opt => opt.MapFrom(src => src.REFISEMRINO));

            CreateMap<RII_FN_PRODUCT_LINE, ProductLineDto>()
                .ForMember(dest => dest.MamulKodu, opt => opt.MapFrom(src => src.MAMUL_KODU))
                .ForMember(dest => dest.MamulAdi, opt => opt.MapFrom(src => src.MAMUL_ADI))
                .ForMember(dest => dest.IsemriNo, opt => opt.MapFrom(src => src.ISEMRINO))
                .ForMember(dest => dest.SiparisNo, opt => opt.MapFrom(src => src.SIPARIS_NO))
                .ForMember(dest => dest.HamKodu, opt => opt.MapFrom(src => src.HAM_KODU))
                .ForMember(dest => dest.HamMaddeAdi, opt => opt.MapFrom(src => src.HAM_MADDE_ADI))
                .ForMember(dest => dest.BirimMiktar, opt => opt.MapFrom(src => src.BIRIM_MIKTAR))
                .ForMember(dest => dest.HesaplananToplamMiktar, opt => opt.MapFrom(src => src.HESAPLANAN_TOPLAM_MIKTAR))
                .ForMember(dest => dest.OpNo, opt => opt.MapFrom(src => src.OPNO));
        }
    }
}
