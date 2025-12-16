# =========================================================
# VERII WMS Server – Cursor AI Kuralları
# =========================================================
# Bu dosya, Cursor AI’nin bu projede kod üretirken
# mimari, domain ve teknik kurallara uymasını sağlar.
# =========================================================

# ---------------------------------------------------------
# 1. GENEL MİMARİ KURALLAR
# ---------------------------------------------------------

- Proje katmanlı mimariye sahiptir:
  Controllers → Services → UnitOfWork/Repositories → Models → Data/Configuration

- Controller:
  - Sadece HTTP orchestration yapar
  - Business rule içermez
  - DbContext veya Repository kullanmaz
  - Transaction başlatmaz

- Service:
  - Business logic burada yazılır
  - Transaction burada yönetilir
  - UnitOfWork üzerinden repository erişimi yapılır

- Repository:
  - Sadece veri erişimi yapar
  - Business kural içermez

- Mapping:
  - AutoMapper zorunludur
  - DTO → Entity manuel mapping YASAK
  - Service veya Controller içinde mapping yazılmaz

# ---------------------------------------------------------
# 2. DOMAIN & AGGREGATE KURALLARI
# ---------------------------------------------------------

- Header bir Aggregate Root’tur.
- Line, LineSerial, ImportLine, Route, TerminalLine
  doğrudan bağımsız domain objesi değildir.

- Alt entity’ler (Line, LineSerial vb.)
  Header veya ilgili Service üzerinden yönetilir.

- Header olmadan alt entity oluşturulamaz.

# ---------------------------------------------------------
# 3. BASE ENTITY & DTO KURALLARI
# ---------------------------------------------------------

- Yeni entity veya DTO yazmadan önce:
  - Base<Entity>Entity
  - Base<Entity>EntityDto
  - Base<Entity>CreateDto
  - Base<Entity>UpdateDto
  mutlaka kontrol edilir.

- Base class kullanılmadan yeni alan eklenmez.

- Create/Update DTO’larda:
  - CustomerName
  - StockName
  - Açıklama (ERP’den gelen)
  gibi gösterim alanları YER ALMAZ.

# ---------------------------------------------------------
# 4. CRUD KULLANIM FELSEFESİ
# ---------------------------------------------------------

- Line ve LineSerial için CRUD endpoint’leri OLABİLİR.
- Ancak bu endpoint’ler:
  - Admin
  - Debug
  - Özel senaryolar
  içindir.

- Operasyonel süreçlerde:
  - Generate
  - BulkGenerate
  - Route akışları
  kullanılır.

- CRUD var ama kullanılmıyor olması HATA DEĞİLDİR.

# ---------------------------------------------------------
# 5. GENERATE & BULK GENERATE KURALLARI
# ---------------------------------------------------------

- Generate süreci sırası:
  1. Header
  2. Lines (HeaderId)
  3. LineSerials (LineId)
  4. ImportLines
  5. Routes (ImportLineId)
  6. TerminalLines

- Bulk Generate işlemlerinde:
  - Client geçici key kullanır:
    HeaderKey, LineKey, ImportLineKey
  - Bu key’ler DB’ye KALICI yazılmaz.
  - Server tarafında gerçek ID’lere map edilir.
  - Hata durumunda tüm işlem rollback edilir.

# ---------------------------------------------------------
# 6. SOFT DELETE KURALLARI
# ---------------------------------------------------------

- Hard delete YASAK.
- Tüm silmeler Soft Delete olarak yapılır.

- Soft Delete öncesi:
  - Route kontrol edilir
  - LineSerial kontrol edilir

- Header:
  - Alt kayıt kalmazsa otomatik soft delete edilir.

- Silme işlemleri:
  - Service katmanında
  - Transaction içinde yapılır.

# ---------------------------------------------------------
# 7. PAGINATION / FILTER / SORT STANDARDI
# ---------------------------------------------------------

- Tüm liste endpoint’leri PagedRequest alır.
- Filtering JSON tabanlıdır.
- String alanlar:
  - Contains
  - StartsWith
  - EndsWith
- Numeric ve Date alanlar:
  - Min / Max aralığı
- Enum alanlar:
  - String olarak gönderilir
  - Backend enum’a convert eder
- Pagination, filtering ve sorting
  tek helper üzerinden uygulanır.

# ---------------------------------------------------------
# 8. VALIDATION & BUSINESS RULE AYRIMI
# ---------------------------------------------------------

- Validation:
  - DTO seviyesinde
  - DataAnnotations veya FluentValidation

- Business rule:
  - Service seviyesinde

- Hata kodları:
  - Validation → 400
  - Business rule → 400
  - Sistem hatası → 500

# ---------------------------------------------------------
# 9. TRANSACTION & UNIT OF WORK
# ---------------------------------------------------------

- Transaction:
  - Service seviyesinde başlatılır
  - UnitOfWork üzerinden yönetilir

- Controller içinde transaction başlatmak YASAK.

- SaveChanges:
  - Transaction içindeyse tek noktadan çağrılır.

# ---------------------------------------------------------
# 10. AUTH & CONTEXT KURALLARI
# ---------------------------------------------------------

- BranchCode, UserId, CompanyId:
  - HttpContext üzerinden alınır.
  - Client tarafından gönderilmez.

- Endpoint’ler:
  - [Authorize] ile korunur.

# ---------------------------------------------------------
# 11. LOGGING & AUDIT
# ---------------------------------------------------------

- CreatedBy, CreatedAt
- UpdatedBy, UpdatedAt
- DeletedBy, DeletedAt
  otomatik doldurulur.

- Exception detayları loglanır,
  client’a sade mesaj döner.

# ---------------------------------------------------------
# 12. AI (CURSOR) DAVRANIŞ KURALLARI
# ---------------------------------------------------------

- Kod üretirken mevcut mimariye UY.
- Aynı işi yapan helper’ları tekrar yazma.
- Base class ve mevcut servisleri kontrol etmeden
  yeni abstraction üretme.
- Mimari dışı veya shortcut çözümler üretme.

# ---------------------------------------------------------
# 13. LOCALIZATION (ZORUNLU KURAL)
# ---------------------------------------------------------

- Projede tüm kullanıcıya dönen mesajlar LOCALIZED olmalıdır.
- Hardcoded string mesaj kullanımı YASAK.

- ApiResponse içerisinde dönen:
  - Message
  - Error
  - Validation mesajları
  mutlaka LocalizationService üzerinden alınır.

# ---------------------------------------------------------
# 13.1 CLASS BAZLI LOCALIZATION
# ---------------------------------------------------------

- Localization key’leri CLASS bazlı gruplanır.
- Her Service veya Controller kendi class adıyla localization region’ına sahiptir.

Örnek:
  Class: GrImportLineService
  Localization Region: GrImportLine

Key formatı:
  <ClassName>.<ActionOrRule>

Örnek key’ler:
  GrImportLine.NotFound
  GrImportLine.RoutesExist
  GrImportLine.LineSerialsExist
  GrImportLine.DeletedSuccessfully
  GrImportLine.ErrorOccurred

- Localization key’leri ASLA global veya karışık isimlendirilmez.

# ---------------------------------------------------------
# 13.2 LOCALIZATION KULLANIM ŞEKLİ
# ---------------------------------------------------------

- Kod içerisinde localization şu şekilde çağrılır:

  _localizationService.GetLocalizedString("GrImportLine.NotFound")

- Aynı mesaj birden fazla yerde kullanılıyorsa
  yine class bazlı key korunur.

# ---------------------------------------------------------
# 13.3 REGION (RESOURCE GROUP) YOKSA OTOMATİK OLUŞTURMA
# ---------------------------------------------------------

- Eğer ilgili class için localization region (resource group) yoksa:
  - Yeni region otomatik olarak oluşturulur.
  - Region adı class adı ile birebir aynı olur.

Örnek:
  Class: GrImportLineService
  → Resource Region: GrImportLine

- Cursor yeni bir Service oluşturduğunda:
  - Aynı isimle localization region açar.
  - Tüm mesajları bu region altına ekler.

# ---------------------------------------------------------
# 13.4 LOCALIZATION FALLBACK KURALI
# ---------------------------------------------------------

- Eğer localization key bulunamazsa:
  - Key string doğrudan kullanıcıya gösterilmez.
  - Fallback olarak İngilizce default mesaj döner.

- Localization eksikliği uygulamanın çalışmasını BOZMAZ
  ancak eksik key loglanır.

# ---------------------------------------------------------
# 13.5 VALIDATION & BUSINESS RULE LOCALIZATION
# ---------------------------------------------------------

- Validation mesajları da localization kullanır.
- DataAnnotations veya FluentValidation mesajları:
  LocalizationService üzerinden resolve edilir.

- Business rule ihlallerinde:
  - Message localization’dan alınır
  - ExceptionMessage sistem loglarına yazılır

# ---------------------------------------------------------
# 13.6 API RESPONSE & LOCALIZATION
# ---------------------------------------------------------

- ApiResponse<T> oluşturulurken:
  - Message alanı mutlaka localized olmalıdır.
  - ExceptionMessage localization içermez (debug/log amaçlıdır).

Örnek:
  ApiResponse<bool>
  .ErrorResult(_localizationService.GetLocalizedString("GrImportLine.RoutesExist"),exceptionMessage,400)

# ---------------------------------------------------------
# 13.7 AI (CURSOR) LOCALIZATION DAVRANIŞI
# ---------------------------------------------------------

- Yeni yazılan her Service için:
  - Aynı isimle localization region oluştur.
  - Mesajları bu region altında grupla.

- Aynı mesajı farklı class’lar arasında paylaşma.
- Generic mesajlar bile class bazlı yazılır.

# =========================================================
# BU DOSYA REFERANS ALINMADAN YAZILAN KOD KABUL EDİLMEZ
# =========================================================
