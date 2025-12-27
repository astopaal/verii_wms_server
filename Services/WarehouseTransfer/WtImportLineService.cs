using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtImportLineService : IWtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtImportLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineNotFound"), _localizationService.GetLocalizedString("WtImportLineNotFound"), 404);
                }

                var dto = _mapper.Map<WtImportLineDto>(entity);
                var enrichedStock = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedStock.Success)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                var finalDto = enrichedStock.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WtImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> CreateAsync(CreateWtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;

                await _unitOfWork.WtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtImportLineDto>(entity);
                return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> UpdateAsync(long id, UpdateWtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineNotFound"), _localizationService.GetLocalizedString("WtImportLineNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.WtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtImportLineDto>(entity);
                return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                // ============================================
                // VALIDATION: Entity existence check
                // ============================================
                var entity = await _unitOfWork.WtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFoundMsg = _localizationService.GetLocalizedString("WtImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(notFoundMsg, notFoundMsg, 404);
                }

                // ============================================
                // VALIDATION: Check for dependent routes
                // ============================================
                var hasRoutes = await _unitOfWork.WtRoutes
                    .AsQueryable()
                    .AnyAsync(r => r.ImportLineId == id && !r.IsDeleted);
                if (hasRoutes)
                {
                    var msg = _localizationService.GetLocalizedString("WtImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                // ============================================
                // VALIDATION: Check for active line serials (only if LineId exists)
                // ============================================
                if (entity.LineId.HasValue)
                {
                    var hasActiveLineSerials = await _unitOfWork.WtLineSerials
                        .AsQueryable()
                        .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId.Value);
                    if (hasActiveLineSerials)
                    {
                        var msg = _localizationService.GetLocalizedString("WtImportLineLineSerialsExist");
                        return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                    }
                }

                // ============================================
                // TRANSACTION: Perform deletion operations
                // ============================================
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WtImportLines.SoftDelete(id);

                    // Check if header should also be deleted
                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.WtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    
                    if (!hasOtherLines)
                    {
                        var hasOtherImportLines = await _unitOfWork.WtImportLines
                            .AsQueryable()
                            .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                        
                        if (!hasOtherImportLines)
                        {
                            await _unitOfWork.WtHeaders.SoftDelete(headerId);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtImportLineDeletedSuccessfully"));
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("WtImportLineErrorOccurred"), 
                    ex.Message ?? string.Empty, 
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByRouteIdAsync(long routeId)
        {
            try
            {
                var routes = await _unitOfWork.WtRoutes.FindAsync(r => r.Id == routeId && !r.IsDeleted);
                var importLineIds = routes.Select(r => r.ImportLineId).ToList();

                var entities = importLineIds.Count == 0
                    ? new List<WtImportLine>()
                    : await _unitOfWork.WtImportLines.FindAsync(x => importLineIds.Contains(x.Id) && !x.IsDeleted);

                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.WtHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderNotFound"), _localizationService.GetLocalizedString("WtHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.WtImportLines
                    .AsQueryable()
                    .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                    .ToListAsync();

                if (!importLines.Any())
                {
                    return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.SuccessResult(
                        Array.Empty<WtImportLineWithRoutesDto>(),
                        _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
                }

                // 1. Tüm routes'ları tek sorguda çek - N+1 problemi çözüldü
                var importLineIds = importLines.Select(il => il.Id).ToList();
                var routes = await _unitOfWork.WtRoutes
                    .AsQueryable()
                    .Where(r => importLineIds.Contains(r.ImportLineId) && !r.IsDeleted)
                    .ToListAsync();

                // 2. Routes'ları ImportLineId'ye göre Dictionary'de grupla - O(1) lookup için
                var routesByImportLineId = routes
                    .GroupBy(r => r.ImportLineId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // 3. Package bilgilerini al ve Dictionary'de sakla - Join ile optimize edilmiş
                Dictionary<long, (long? PackageLineId, string? PackageNo, long? packageHeaderId)> packageInfoDict = new();
                if (routes.Count > 0)
                {
                    var routeIds = routes.Select(r => r.Id).ToList();
                    var packageInfoList = await (
                        from pl in _unitOfWork.PLines.AsQueryable()
                        join p in _unitOfWork.PPackages.AsQueryable() on pl.PackageId equals p.Id
                        join ph in _unitOfWork.PHeaders.AsQueryable() on p.PackingHeaderId equals ph.Id
                        where !pl.IsDeleted
                              && !p.IsDeleted
                              && !ph.IsDeleted
                              && pl.SourceRouteId.HasValue
                              && routeIds.Contains(pl.SourceRouteId.Value)
                              && ph.SourceHeaderId == headerId
                              && ph.SourceType == "WT"
                        select new
                        {
                            RouteId = pl.SourceRouteId!.Value,
                            PackageLineId = pl.Id,
                            PackageNo = p.PackageNo,
                            packageHeaderId = ph.Id
                        }
                    ).ToListAsync();

                    packageInfoDict = packageInfoList
                        .GroupBy(x => x.RouteId)
                        .ToDictionary(g => g.Key, g => (g?.First().PackageLineId, g?.First().PackageNo, g?.First().packageHeaderId));
                }

                // 4. Dictionary'leri kullanarak DTO'ları oluştur - O(1) lookup
                var items = importLines.Select(importLine =>
                {
                    var importLineDto = _mapper.Map<WtImportLineDto>(importLine);

                    var routeDtos = routesByImportLineId
                        .GetValueOrDefault(importLine.Id, new List<WtRoute>())
                        .Select(route =>
                        {
                            var routeDto = _mapper.Map<WtRouteDto>(route);
                            if (packageInfoDict.TryGetValue(route.Id, out var packageInfo))
                            {
                                routeDto.PackageLineId = packageInfo.PackageLineId;
                                routeDto.PackageNo = packageInfo.PackageNo;
                                routeDto.PackageHeaderId = packageInfo.packageHeaderId;
                            }
                            return routeDto;
                        })
                        .ToList();

                    return new WtImportLineWithRoutesDto
                    {
                        ImportLine = importLineDto,
                        Routes = routeDtos
                    };
                }).ToList();

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(
                        enriched.Message,
                        enriched.ExceptionMessage,
                        enriched.StatusCode
                    );
                }
                var enrichedList = enriched.Data?.ToList() ?? importLineDtos;
                for (int i = 0; i < items.Count && i < enrichedList.Count; i++)
                {
                    items[i].ImportLine = enrichedList[i];
                }

                return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWtImportBarcodeRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        // ============================================
                        // 1) MİKTAR DOĞRULAMA: Negatif/0 miktara izin verilmez
                        // ============================================
                        if (request.Quantity <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<WtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtImportLineQuantityInvalid"), 
                                _localizationService.GetLocalizedString("WtImportLineQuantityInvalid"), 
                                400);
                        }

                        // ============================================
                        // 2) HEADER KONTROLÜ: İstekle gelen header aktif ve silinmemiş olmalı
                        // ============================================
                        var header = await _unitOfWork.WtHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<WtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtHeaderNotFound"), 
                                _localizationService.GetLocalizedString("WtHeaderNotFound"), 
                                404);
                        }

                        // ============================================
                        // 3) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi ile header'a ait silinmemiş Line'ları bul
                        // ============================================
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        
                        var matchingLines = await _unitOfWork.WtLines
                            .AsQueryable()
                            .Where(l => l.HeaderId == request.HeaderId 
                                && !l.IsDeleted
                                && ((l.StockCode ?? "").Trim() == reqStock)
                                && ((l.YapKod ?? "").Trim() == reqYap))
                            .ToListAsync();
                        
                        if (!matchingLines.Any())
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<WtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtImportLineStokCodeAndYapCodeNotMatch"), 
                                _localizationService.GetLocalizedString("WtImportLineStokCodeAndYapCodeNotMatch"), 
                                404);
                        }

                        // ============================================
                        // 4) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                        // ============================================
                        var serialNo = (request.SerialNo ?? "").Trim();
                        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                        // Eşleşen Line'ların (StockCode + YapKod ile eşleşen) LineSerial'larını kontrol et
                        var lineIds = matchingLines.Select(l => l.Id).ToList();
                        var lineSerials = await _unitOfWork.WtLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && lineIds.Contains(ls.LineId))
                            .ToListAsync();

                        // Eşleşen Line'ların LineSerial'larında SerialNo var mı kontrol et
                        var hasSerialInLineSerials = lineSerials.Any(ls =>
                            !string.IsNullOrWhiteSpace(ls.SerialNo));

                        // Get WtParameter for validation rules
                        var wtParameter = await _unitOfWork.WtParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        {
                            // ============================================
                            // DURUM 1: Her ikisinde de SerialNo var → Seri eşleşmesi + Seri bazlı miktar kontrolü
                            // ============================================
                            if (hasSerialInLineSerials && hasRequestSerial)
                            {
                                var matchingLineSerials = lineSerials.Where(ls =>
                                    ((ls.SerialNo ?? "").Trim() == serialNo)
                                ).ToList();

                                if (!matchingLineSerials.Any())
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<WtImportLineDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtImportLineSerialNotMatch"), 
                                        _localizationService.GetLocalizedString("WtImportLineSerialNotMatch"), 
                                        404);
                                }

                                // Seri bazlı miktar kontrolü
                                var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
                                
                                var totalRouteQuantity = await _unitOfWork.WtRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted
                                        && ((r.SerialNo ?? "").Trim() == serialNo))
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = wtParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("WtHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"Serial {serialNo} (StockCode: {reqStock}, YapKod: {reqYap}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<WtImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
                                }
                            }
                            // ============================================
                            // DURUM 2: LineSerial'da SerialNo yok VEYA Request'te SerialNo yok → Toplam miktar kontrolü (seri bazlı değil)
                            // ============================================
                            else
                            {
                                // Tüm LineSerial'ların toplam miktarı
                                var totalLineSerialQuantity = lineSerials.Sum(ls => ls.Quantity);

                                // Tüm Route'ların toplam miktarı (eşleşen Line'lar için)
                                var totalRouteQuantity = await _unitOfWork.WtRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted)
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = wtParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("WtHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"StockCode: {reqStock}, YapKod: {reqYap}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<WtImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
                                }
                            }
                        }

                        // ============================================
                        // 5) IMPORTLINE BUL/OLUŞTUR: En uygun Line'ı seç ve ImportLine bul/oluştur
                        // ============================================
                        long? selectedLineId = null;

                        // Eğer seri varsa ve sadece bir Line'da o seri varsa → O Line'ı seç
                        if (hasSerialInLineSerials && hasRequestSerial)
                        {
                            var linesWithSerial = lineSerials
                                .Where(ls => ((ls.SerialNo ?? "").Trim() == serialNo))
                                .Select(ls => ls.LineId)
                                .Distinct()
                                .ToList();

                            if (linesWithSerial.Count == 1)
                            {
                                // Sadece bir Line'da bu seri var → O Line'ı seç
                                selectedLineId = linesWithSerial.First();
                            }
                            // Eğer birden fazla Line'da aynı seri varsa → Seri mantığı geçersiz, toplam miktar mantığına geç
                        }

                        // Eğer Line seçilmediyse (seri yok veya birden fazla Line'da seri var) → En fazla eksik olan Line'ı seç
                        if (!selectedLineId.HasValue)
                        {
                            var lineQuantities = new List<(long LineId, decimal LineSerialTotal, decimal RouteTotal, decimal Remaining)>();

                            foreach (var line in matchingLines)
                            {
                                // LineSerial toplam miktarı
                                var lineSerialTotal = lineSerials
                                    .Where(ls => ls.LineId == line.Id)
                                    .Sum(ls => ls.Quantity);

                                // Route toplam miktarı (bu Line'a bağlı ImportLine'ların Route'ları)
                                var routeTotal = await _unitOfWork.WtRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && r.ImportLine.LineId == line.Id
                                        && !r.ImportLine.IsDeleted)
                                    .SumAsync(r => r.Quantity);

                                var remaining = lineSerialTotal - routeTotal;
                                lineQuantities.Add((line.Id, lineSerialTotal, routeTotal, remaining));
                            }

                            // En fazla eksik olan Line'ı seç (remaining en yüksek olan)
                            var bestLine = lineQuantities
                                .OrderByDescending(x => x.Remaining)
                                .FirstOrDefault();

                            if (bestLine.LineId > 0)
                            {
                                selectedLineId = bestLine.LineId;
                            }
                            else
                            {
                                // Fallback: İlk eşleşen Line'ı seç
                                selectedLineId = matchingLines.First().Id;
                            }
                        }

                        // Seçilen Line için ImportLine bul veya oluştur
                        if (!selectedLineId.HasValue)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<WtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtImportLineNoMatchingLine"), 
                                _localizationService.GetLocalizedString("WtImportLineNoMatchingLine"), 
                                400);
                        }

                        WtImportLine? importLine = await _unitOfWork.WtImportLines
                            .AsQueryable()
                            .FirstOrDefaultAsync(il => il.HeaderId == request.HeaderId
                                && il.LineId == selectedLineId.Value
                                && ((il.StockCode ?? "").Trim() == reqStock)
                                && ((il.YapKod ?? "").Trim() == reqYap)
                                && !il.IsDeleted);

                        if (importLine == null)
                        {
                            var createImportLineDto = new CreateWtImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = selectedLineId.Value,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<WtImportLine>(createImportLineDto);
                            await _unitOfWork.WtImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 6) ROUTE KAYDI: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        // ============================================
                        var createRouteDto = new CreateWtRouteDto
                        {
                            ImportLineId = importLine.Id,
                            ScannedBarcode = request.Barcode,
                            Quantity = request.Quantity,
                            SerialNo = request.SerialNo,
                            SerialNo2 = request.SerialNo2,
                            SerialNo3 = request.SerialNo3,
                            SerialNo4 = request.SerialNo4,
                            SourceCellCode = request.SourceCellCode,
                            TargetCellCode = request.TargetCellCode
                        };
                        var route = _mapper.Map<WtRoute>(createRouteDto);

                        await _unitOfWork.WtRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // ============================================
                        // 7) SONUÇ: importLine DTO döndürülür ve işlem tamamlanır
                        // ============================================
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<WtImportLineDto>(importLine);
                        return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
                    }
                    catch
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


    }
}
