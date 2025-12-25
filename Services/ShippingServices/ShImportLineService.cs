using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShImportLineService : IShImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public ShImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<ShImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<ShImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShImportLine>(createDto);
                var created = await _unitOfWork.ShImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(created);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(entity);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var routes = await _unitOfWork.ShRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("ShImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var hasActiveLineSerials = await _unitOfWork.ShLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("ShImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.ShImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.ShLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.ShImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.ShHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.ShHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderNotFound"), _localizationService.GetLocalizedString("ShHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var items = new List<ShImportLineWithRoutesDto>();

                foreach (var il in importLines)
                {
                    var routes = await _unitOfWork.ShRoutes.FindAsync(r => r.ImportLineId == il.Id && !r.IsDeleted);
                    var dto = new ShImportLineWithRoutesDto
                    {
                        ImportLine = _mapper.Map<ShImportLineDto>(il),
                        Routes = _mapper.Map<List<ShRouteDto>>(routes)
                    };
                    items.Add(dto);
                }

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(
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

                return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddShImportBarcodeRequestDto request)
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
                            return ApiResponse<ShImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("ShImportLineQuantityInvalid"), 
                                _localizationService.GetLocalizedString("ShImportLineQuantityInvalid"), 
                                400);
                        }

                        // ============================================
                        // 2) HEADER KONTROLÜ: İstekle gelen header aktif ve silinmemiş olmalı
                        // ============================================
                        var header = await _unitOfWork.ShHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<ShImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("ShHeaderNotFound"), 
                                _localizationService.GetLocalizedString("ShHeaderNotFound"), 
                                404);
                        }

                        // ============================================
                        // 3) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi ile header'a ait silinmemiş Line'ları bul
                        // ============================================
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        
                        var matchingLines = await _unitOfWork.ShLines
                            .AsQueryable()
                            .Where(l => l.HeaderId == request.HeaderId 
                                && !l.IsDeleted
                                && ((l.StockCode ?? "").Trim() == reqStock)
                                && ((l.YapKod ?? "").Trim() == reqYap))
                            .ToListAsync();
                        
                        if (!matchingLines.Any())
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<ShImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("ShImportLineStokCodeAndYapCodeNotMatch"), 
                                _localizationService.GetLocalizedString("ShImportLineStokCodeAndYapCodeNotMatch"), 
                                404);
                        }

                        // ============================================
                        // 4) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                        // ============================================
                        var serialNo = (request.SerialNo ?? "").Trim();
                        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                        // Eşleşen Line'ların (StockCode + YapKod ile eşleşen) LineSerial'larını kontrol et
                        var lineIds = matchingLines.Select(l => l.Id).ToList();
                        var lineSerials = await _unitOfWork.ShLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && lineIds.Contains(ls.LineId))
                            .ToListAsync();

                        // Eşleşen Line'ların LineSerial'larında SerialNo var mı kontrol et
                        var hasSerialInLineSerials = lineSerials.Any(ls =>
                            !string.IsNullOrWhiteSpace(ls.SerialNo));

                        // Get ShParameter for validation rules
                        var shParameter = await _unitOfWork.ShParameters
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
                                    return ApiResponse<ShImportLineDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("ShImportLineSerialNotMatch"), 
                                        _localizationService.GetLocalizedString("ShImportLineSerialNotMatch"), 
                                        404);
                                }

                                // Seri bazlı miktar kontrolü
                                var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
                                
                                var totalRouteQuantity = await _unitOfWork.ShRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted
                                        && ((r.SerialNo ?? "").Trim() == serialNo))
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = shParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("ShHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"Serial {serialNo} (StockCode: {reqStock}, YapKod: {reqYap}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<ShImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var totalRouteQuantity = await _unitOfWork.ShRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted)
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = shParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("ShHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"StockCode: {reqStock}, YapKod: {reqYap}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<ShImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var routeTotal = await _unitOfWork.ShRoutes
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
                            return ApiResponse<ShImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("ShImportLineNoMatchingLine"), 
                                _localizationService.GetLocalizedString("ShImportLineNoMatchingLine"), 
                                400);
                        }

                        ShImportLine? importLine = await _unitOfWork.ShImportLines
                            .AsQueryable()
                            .FirstOrDefaultAsync(il => il.HeaderId == request.HeaderId
                                && il.LineId == selectedLineId.Value
                                && ((il.StockCode ?? "").Trim() == reqStock)
                                && ((il.YapKod ?? "").Trim() == reqYap)
                                && !il.IsDeleted);

                        if (importLine == null)
                        {
                            var createImportLineDto = new CreateShImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = selectedLineId.Value,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<ShImportLine>(createImportLineDto);
                            await _unitOfWork.ShImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 6) ROUTE KAYDI: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        // ============================================
                        var createRouteDto = new CreateShRouteDto
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
                        var route = _mapper.Map<ShRoute>(createRouteDto);

                        await _unitOfWork.ShRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // ============================================
                        // 7) SONUÇ: importLine DTO döndürülür ve işlem tamamlanır
                        // ============================================
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<ShImportLineDto>(importLine);
                        return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
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
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
