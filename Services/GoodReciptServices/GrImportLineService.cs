using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrImportLineService : IGrImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public GrImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<GrImportLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.GrImportLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<GrImportLineDto>>(items);

                var result = new PagedResponse<GrImportLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<GrImportLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync()
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.GetAllAsync();
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportLineDto?>> GetByIdAsync(long id)
        {
            try
            {
                var grImportLine = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (grImportLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<GrImportLineDto?>.ErrorResult(nf, nf, 404);
                }

                var grImportLineDto = _mapper.Map<GrImportLineDto>(grImportLine);

                var enriched = await _erpService.PopulateStockNamesAsync(new[] { grImportLineDto });
                if (!enriched.Success)
                {
                    return ApiResponse<GrImportLineDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? grImportLineDto;
                
                return ApiResponse<GrImportLineDto?>.SuccessResult(finalDto, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto?>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId)
        {
            try
            {
                var items = await _unitOfWork.GrImportLines
                    .AsQueryable()
                    .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                    .Include(x => x.Routes.Where(r => !r.IsDeleted))
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<GrImportLineWithRoutesDto>>(items);

                var enrichedResp = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedResp.Success)
                {
                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.ErrorResult(enrichedResp.Message, enrichedResp.ExceptionMessage, enrichedResp.StatusCode);
            }
                
                dtos = enrichedResp.Data ?? dtos;

                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var grImportLines = await _unitOfWork.GrImportLines.FindAsync(x => x.LineId == lineId);
                var grImportLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(grImportLines);

                var enriched = await _erpService.PopulateStockNamesAsync(grImportLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                
                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? grImportLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }


        public async Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto)
        {
            try
            {
                var grImportLine = _mapper.Map<GrImportLine>(createDto);
                grImportLine.CreatedDate = DateTime.UtcNow;
                
                await _unitOfWork.GrImportLines.AddAsync(grImportLine);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLineDto = _mapper.Map<GrImportLineDto>(grImportLine);
                
                return ApiResponse<GrImportLineDto>.SuccessResult(grImportLineDto, _localizationService.GetLocalizedString("GrImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportLineDto>> UpdateAsync(long id, UpdateGrImportLineDto updateDto)
        {
            try
            {
                var existingGrImportLine = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (existingGrImportLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<GrImportLineDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, existingGrImportLine);
                existingGrImportLine.UpdatedDate = DateTime.UtcNow;
                
                _unitOfWork.GrImportLines.Update(existingGrImportLine);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLineDto = _mapper.Map<GrImportLineDto>(existingGrImportLine);
                
                return ApiResponse<GrImportLineDto>.SuccessResult(grImportLineDto, _localizationService.GetLocalizedString("GrImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                var routes = await _unitOfWork.GrRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("GrImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                var hasActiveLineSerials = entity.LineId.HasValue && await _unitOfWork.GrLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId.Value);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("GrImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.GrImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.GrLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.GrImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.GrHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineDeletionError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetImportLinesByHeaderIdAsync(long headerId)
        {
            try
            {
                var importLines = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var importLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(importLines);

                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(enriched.Data ?? importLineDtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddGrImportBarcodeRequestDto request)
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
                            return ApiResponse<GrImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("GrImportLineQuantityInvalid"), 
                                _localizationService.GetLocalizedString("GrImportLineQuantityInvalid"), 
                                400);
                        }

                        // ============================================
                        // 2) HEADER KONTROLÜ: İstekle gelen header aktif ve silinmemiş olmalı
                        // ============================================
                        var header = await _unitOfWork.GrHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<GrImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("GrHeaderNotFound"), 
                                _localizationService.GetLocalizedString("GrHeaderNotFound"), 
                                404);
                        }

                        // ============================================
                        // 3) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi ile header'a ait silinmemiş Line'ları bul
                        // ============================================
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        
                        var matchingLines = await _unitOfWork.GrLines
                            .AsQueryable()
                            .Where(l => l.HeaderId == request.HeaderId 
                                && !l.IsDeleted
                                && ((l.StockCode ?? "").Trim() == reqStock)
                                && ((l.YapKod ?? "").Trim() == reqYap))
                            .ToListAsync();
                        
                        if (!matchingLines.Any())
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<GrImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("GrImportLineStokCodeAndYapCodeNotMatch"), 
                                _localizationService.GetLocalizedString("GrImportLineStokCodeAndYapCodeNotMatch"), 
                                404);
                        }

                        // ============================================
                        // 4) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                        // ============================================
                        var serialNo = (request.SerialNo ?? "").Trim();
                        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                        // Eşleşen Line'ların (StockCode + YapKod ile eşleşen) LineSerial'larını kontrol et
                        var lineIds = matchingLines.Select(l => l.Id).ToList();
                        var lineSerials = await _unitOfWork.GrLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId.HasValue && lineIds.Contains(ls.LineId.Value))
                            .ToListAsync();

                        // Eşleşen Line'ların LineSerial'larında SerialNo var mı kontrol et
                        var hasSerialInLineSerials = lineSerials.Any(ls =>
                            !string.IsNullOrWhiteSpace(ls.SerialNo));

                        // Get GrParameter for validation rules
                        var grParameter = await _unitOfWork.GrParameters
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
                                    return ApiResponse<GrImportLineDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("GrImportLineSerialNotMatch"), 
                                        _localizationService.GetLocalizedString("GrImportLineSerialNotMatch"), 
                                        404);
                                }

                                // Seri bazlı miktar kontrolü
                                var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
                                
                                var totalRouteQuantity = await _unitOfWork.GrRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted
                                        && ((r.SerialNo ?? "").Trim() == serialNo))
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = grParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("GrHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"Serial {serialNo} (StockCode: {reqStock}, YapKod: {reqYap}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<GrImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var totalRouteQuantity = await _unitOfWork.GrRoutes
                                    .AsQueryable()
                                    .Where(r => !r.IsDeleted
                                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                                        && !r.ImportLine.IsDeleted)
                                    .SumAsync(r => r.Quantity);

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + request.Quantity;
                                bool allowMore = grParameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("GrHeaderQuantityCannotBeGreater", 
                                        matchingLines.First().Id, 
                                        matchingLines.First().StockCode ?? string.Empty, 
                                        matchingLines.First().YapKod ?? string.Empty, 
                                        totalLineSerialQuantity, 
                                        totalRouteQuantityAfterAdd);
                                    var exceptionMessage = $"StockCode: {reqStock}, YapKod: {reqYap}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<GrImportLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
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
                                var routeTotal = await _unitOfWork.GrRoutes
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

                            if (bestLine != default && bestLine.LineId > 0)
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
                            return ApiResponse<GrImportLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("GrImportLineNoMatchingLine"), 
                                _localizationService.GetLocalizedString("GrImportLineNoMatchingLine"), 
                                400);
                        }

                        GrImportLine? importLine = await _unitOfWork.GrImportLines
                            .AsQueryable()
                            .FirstOrDefaultAsync(il => il.HeaderId == request.HeaderId
                                && il.LineId == selectedLineId.Value
                                && ((il.StockCode ?? "").Trim() == reqStock)
                                && ((il.YapKod ?? "").Trim() == reqYap)
                                && !il.IsDeleted);

                        if (importLine == null)
                        {
                            var createImportLineDto = new CreateGrImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = selectedLineId.Value,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<GrImportLine>(createImportLineDto);
                            await _unitOfWork.GrImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 6) ROUTE KAYDI: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        // ============================================
                        var createRouteDto = new CreateGrRouteDto
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
                        var route = _mapper.Map<GrRoute>(createRouteDto);

                        await _unitOfWork.GrRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // ============================================
                        // 7) SONUÇ: importLine DTO döndürülür ve işlem tamamlanır
                        // ============================================
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<GrImportLineDto>(importLine);
                        return ApiResponse<GrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineCreatedSuccessfully"));
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
                return ApiResponse<GrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
        
    }
}

