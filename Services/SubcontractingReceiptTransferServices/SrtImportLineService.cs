using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtImportLineService : ISrtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SrtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }




        public async Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtImportLine>(createDto);
                await _unitOfWork.SrtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var routes = await _unitOfWork.SrtRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("SrtImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var hasActiveLineSerials = await _unitOfWork.SrtLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("SrtImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.SrtImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.SrtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.SrtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.SrtHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.SrtHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderNotFound"), _localizationService.GetLocalizedString("SrtHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.SrtImportLines
                    .AsQueryable()
                    .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                    .ToListAsync();

                if (!importLines.Any())
                {
                    return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.SuccessResult(
                        Array.Empty<SrtImportLineWithRoutesDto>(),
                        _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
                }

                // 1. Tüm routes'ları tek sorguda çek - N+1 problemi çözüldü
                var importLineIds = importLines.Select(il => il.Id).ToList();
                var routes = await _unitOfWork.SrtRoutes
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
                              && ph.SourceType == PHeaderSourceType.SRT
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
                    var importLineDto = _mapper.Map<SrtImportLineDto>(importLine);

                    var routeDtos = routesByImportLineId
                        .GetValueOrDefault(importLine.Id, new List<SrtRoute>())
                        .Select(route =>
                        {
                            var routeDto = _mapper.Map<SrtRouteDto>(route);
                            if (packageInfoDict.TryGetValue(route.Id, out var packageInfo))
                            {
                                routeDto.PackageLineId = packageInfo.PackageLineId;
                                routeDto.PackageNo = packageInfo.PackageNo;
                                routeDto.PackageHeaderId = packageInfo.packageHeaderId;
                            }
                            return routeDto;
                        })
                        .ToList();

                    return new SrtImportLineWithRoutesDto
                    {
                        ImportLine = importLineDto,
                        Routes = routeDtos
                    };
                }).ToList();

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.ErrorResult(
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

                return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request)
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
                            return ApiResponse<SrtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("SrtImportLineQuantityInvalid"), 
                                _localizationService.GetLocalizedString("SrtImportLineQuantityInvalid"), 
                                400);
                        }

                        // ============================================
                        // 2) HEADER KONTROLÜ: İstekle gelen header aktif ve silinmemiş olmalı
                        // ============================================
                        var header = await _unitOfWork.SrtHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<SrtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("SrtHeaderNotFound"), 
                                _localizationService.GetLocalizedString("SrtHeaderNotFound"), 
                                404);
                        }

                        // ============================================
                        // 3) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi ile header'a ait silinmemiş Line'ları bul
                        // ============================================
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        
                        var matchingLines = await _unitOfWork.SrtLines
                            .AsQueryable()
                            .Where(l => l.HeaderId == request.HeaderId 
                                && !l.IsDeleted
                                && ((l.StockCode ?? "").Trim() == reqStock)
                                && ((l.YapKod ?? "").Trim() == reqYap))
                            .ToListAsync();
                        
                        if (!matchingLines.Any())
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<SrtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("SrtImportLineStokCodeAndYapCodeNotMatch"), 
                                _localizationService.GetLocalizedString("SrtImportLineStokCodeAndYapCodeNotMatch"), 
                                404);
                        }

                        // ============================================
                        // 4) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                        // ============================================
                        var serialNo = (request.SerialNo ?? "").Trim();
                        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                        // Eşleşen Line'ların (StockCode + YapKod ile eşleşen) LineSerial'larını kontrol et
                        var lineIds = matchingLines.Select(l => l.Id).ToList();
                        var lineSerials = await _unitOfWork.SrtLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && lineIds.Contains(ls.LineId))
                            .ToListAsync();

                        // Eşleşen Line'ların LineSerial'larında SerialNo var mı kontrol et
                        var hasSerialInLineSerials = lineSerials.Any(ls =>
                            !string.IsNullOrWhiteSpace(ls.SerialNo));

                        // Get SrtParameter for validation rules
                        var srtParameter = await _unitOfWork.SrtParameters
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
                                    return ApiResponse<SrtImportLineDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("SrtImportLineSerialNotMatch"), 
                                        _localizationService.GetLocalizedString("SrtImportLineSerialNotMatch"), 
                                        404);
                                }

                                // Seri bazlı miktar kontrolü
                                var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
                                
                                var totalRouteQuantity = await _unitOfWork.SrtRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted
                                        && ((r.SerialNo ?? "").Trim() == serialNo))
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = srtParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("SrtHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"Serial {serialNo} (StockCode: {reqStock}, YapKod: {reqYap}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<SrtImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var totalRouteQuantity = await _unitOfWork.SrtRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted)
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = srtParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("SrtHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"StockCode: {reqStock}, YapKod: {reqYap}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<SrtImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var routeTotal = await _unitOfWork.SrtRoutes
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
                            return ApiResponse<SrtImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("SrtImportLineNoMatchingLine"), 
                                _localizationService.GetLocalizedString("SrtImportLineNoMatchingLine"), 
                                400);
                        }

                        SrtImportLine? importLine = await _unitOfWork.SrtImportLines
                            .AsQueryable()
                            .FirstOrDefaultAsync(il => il.HeaderId == request.HeaderId
                                && il.LineId == selectedLineId.Value
                                && ((il.StockCode ?? "").Trim() == reqStock)
                                && ((il.YapKod ?? "").Trim() == reqYap)
                                && !il.IsDeleted);

                        if (importLine == null)
                        {
                            var createImportLineDto = new CreateSrtImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = selectedLineId.Value,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<SrtImportLine>(createImportLineDto);
                            await _unitOfWork.SrtImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 6) ROUTE KAYDI: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        // ============================================
                        var createRouteDto = new CreateSrtRouteDto
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
                        var route = _mapper.Map<SrtRoute>(createRouteDto);

                        await _unitOfWork.SrtRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // ============================================
                        // 7) SONUÇ: importLine DTO döndürülür ve işlem tamamlanır
                        // ============================================
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<SrtImportLineDto>(importLine);
                        return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
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
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
