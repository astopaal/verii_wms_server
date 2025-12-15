# VERII WMS Server – Cursor Proje Kuralları ve Yol Haritası

## Amaç
- Kod üretiminde tutarlılığı sağlamak, modül eklemeyi hızlandırmak ve AI asistanların doğru kararlar almasını kolaylaştırmak.
- Projedeki ana kavramları ve veri akışını açıkça tanımlamak.

## Genel Mimari
- Katmanlar: `Controllers` → `Services` → `UnitOfWork/Repositories` → `Models` → `Data/Configuration`
- Veri Transferi: `DTOs` + `AutoMapper` profilleri
- Kimlik: `[Authorize]` ile endpoint erişim kontrolü
- ERP Entegrasyonu: `IErpService` ile müşteri ve depo isim zenginleştirmeleri

## Temel Varlıklar
- Header: Üst kayıt. Planlama, durum ve doküman bilgilerini taşır. Tüm modüllerde ana yapı.
- Line: Emre bağlı sipariş satırı. Ürün, miktar, birim ve ERP referansları.
- LineSerial: Emre bağlı seri/miktar hareketi. SeriNo(1-4), miktar ve hücre bilgileri.
- ImportLine: Toplanan ürün satırı. Stok kodu ve açıklamalar; ilgili Line bağlantısı.
- Route: ImportLine’a bağlı miktar, seri, depo/hücre bilgisi ve yönlendirme adımı.
- TerminalLine: Emre bağlı işlem yapacak kullanıcı listesi.

## Base Sınıflar ve DTO Üçlüsü
- Header
  - Model: `BaseHeaderEntity`
  - DTO: `BaseHeaderEntityDto`
  - Create: `BaseHeaderCreateDto`
  - Update: `BaseHeaderUpdateDto`
- Line
  - Model: `BaseLineEntity`
  - DTO: `BaseLineEntityDto`
  - Create: `BaseLineCreateDto`
  - Update: `BaseLineUpdateDto`
- LineSerial
  - Model: `BaseLineSerialEntity`
  - DTO: `BaseLineSerialEntityDto`
  - Create: `BaseLineSerialCreateDto`
  - Update: `BaseLineSerialUpdateDto`
- ImportLine
  - Model: `BaseImportLineEntity`
  - DTO: `BaseImportLineEntityDto`
  - Create: `BaseImportLineCreateDto`
  - Update: `BaseImportLineUpdateDto`
- Route
  - Model: `BaseRouteEntity`
  - DTO: `BaseRouteEntityDto`
  - Create: `BaseRouteCreateDto`
  - Update: `BaseRouteUpdateDto`

## DTO Alan Kullanım Kuralları
- Create/Update DTO’larda `CustomerName`, `StockName`, `YapAcik` alanları yer almaz.
- Entity DTO’larda gösterim amaçlı alanlar bulunabilir; operasyonel alanlar Create/Update’a taşınmaz.
- Route Create/Update DTO’ları sadece yönlendirme için gerekli alanları (`Quantity`, `SerialNo*`, `Source/TargetWarehouse`, `Source/TargetCellCode`) içerir.
- ImportLine Create/Update DTO’ları stok kodu ve açıklamaları base’den alır; ilişki anahtarlarını (HeaderId, LineId, RouteId?) modüle göre ekler.

## Modül Ekleme Rehberi
- Modül adları: `WarehouseTransfer`, `WarehouseOutbound`, `WarehouseInbound`, `ProductionTransfer`, `SubcontractingIssueTransfer`, `SubcontractingReceiptTransfer`, `InventoryCount`.
- Adımlar:
  - Models: Header/Line/ImportLine/Route/TerminalLine sınıflarını base’den türet.
  - Data/Configuration: Her model için tablo ve kolon kurallarını ekle, index ve ilişkileri tanımla.
  - DTOs: Entity/ Create/ Update üçlüsünü base’den türeterek yaz, gereksiz alanları ekleme.
  - Mappings: `Profile` oluştur, Create/Update DTO → Model ve Model → DTO eşlemelerini tanımla.
  - Services: `I<Service>` arayüzü ve `Service` implementasyonu; `UnitOfWork` üzerinden CRUD ve süreçler.
  - Controllers: `[Authorize]` ile korunan REST endpoint’leri ekle; `generate`, `bulk-generate` paternini uygula.

## Servis Kuralları
- `UnitOfWork` ile repository’lere eriş; transaction gerektiren işlerde `BeginTransactionAsync` kullan.
- `ApiResponse<T>` ile tutarlı cevap şablonu; `LocalizationService` ile mesajlar.
- Zenginleştirme: ERP servisleri ile müşteri/depo isimleri doldurulur; başarısızlıkta hata döndürülür.
- `CompleteAsync`, `SetApprovalAsync` gibi durum değişiklikleri tek noktadan yönetilir.

## Controller Kuralları
- Route örnekleri:
  - Header: `GET/POST/PUT/DELETE`, `POST generate`, `POST bulk-generate`, `POST approval/{id}`
  - Lines: `POST`, `PUT`, `DELETE`
  - LineSerials: `POST`, `PUT`, `DELETE`
  - ImportLines: `POST`, `PUT`, `DELETE`
  - Routes: `POST`, `PUT`, `DELETE`
  - TerminalLines: `POST`, `PUT`, `DELETE`
- `ModelState.IsValid` kontrolü ve doğru `StatusCode` dönüşleri.

## Mapping Kuralları
- Her aggregate için ayrı `Profile`.
- Base alanlar otomatik eşlenir; modül spesifik alanlar açıkça belirtilir.
- Kaynaktaki null üyeler `ForAllMembers(... Condition ...)` ile hedefe yazılmaz.

## Validasyon ve İsimlendirme
- `DataAnnotations` ile `Required`, `StringLength`, `MaxLength` kullan.
- İlişki anahtarları: `HeaderId`, `LineId`, `ImportLineId`, `RouteId` tutarlı adlandırılır.
- Create zorunlu alanları net belirt; Update opsiyonel (`nullable`) olmalıdır.

## Süreç Paternleri
- Generate:
  - Header oluştur
  - Lines ekle ve `HeaderId` ile bağla
  - LineSerials ekle ve `LineId` ile bağla
  - ImportLines ekle ve gerekli yerlere bağla
  - Routes ekle ve `ImportLineId` ile bağla
  - TerminalLines ekle ve kullanıcıları ilişkilendir
- Bulk Generate:
  - `HeaderKey`/`LineKey`/`ImportLineKey` gibi istemci anahtarlarını server ID’lerine eşle
  - Hata durumunda rollback

## Güvenlik ve Uygulama Kuralları
- `[Authorize]` zorunlu; gizli bilgiler logging’e yazılmaz.
- Exception mesajları son kullanıcıya sade, sistem loglarına detay eklenir.
- Soft delete kullanımlarında `IsDeleted` dikkate alınır; sorgular bu filtreyle yazılır.

## Çalıştırma ve Doğrulama
- Build: `dotnet build VERII_WMS_WEBAPI.sln`
- Çalıştırma komutları ve test çerçevesi eklenirse bu dosya güncellenecek.

## Terimler Sözlüğü
- Header: Üst kayıt; planlama ve doküman meta verisi. Base header ana yapısı.
- Line: Sipariş içeren emir tablosu; stok kodu, miktar, birim, ERP referansları.
- LineSerial: Emre bağlı siparişin miktar/seri bilgisi; seriNo(1–4) ve hücre kodları.
- ImportLine: Toplanan ürünler; stok kodu ve açıklamalar; line’a bağlı toplama kaydı.
- Route: ImportLine’ye bağlı miktar, seri ve raf (hücre) bilgisi; akış adımı.
- TerminalLine: Emre bağlı işlem yapacak kullanıcı listesi.

## Kod Kalitesi ve Teknik Borç
- Base DTO ve entity sınıfları kullanılmadan yeni alan eklenmez.
- Create/Update DTO’larda operasyon dışı (gösterim amaçlı) alan bulundurulmaz.
- Profil ve servislerde kaldırılan alanlara eşleme/kullanım eklenmez.

