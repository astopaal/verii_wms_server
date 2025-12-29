using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PHeaderService : IPHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PHeaderDto>>> GetAllAsync()
        {
            try
            {
                var headers = await _unitOfWork.PHeaders.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PHeaderDto>>(headers);
                
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PHeaders.AsQueryable();
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<PHeaderDto>>(items);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                
                var result = new PagedResponse<PHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto?>> GetByIdAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PHeaderDto?>.ErrorResult(nf, nf, 404);
                }

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto?>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto>> CreateAsync(CreatePHeaderDto createDto)
        {
            try
            {
                // PackingNo unique kontrolü - silinmemiş kayıt varsa hata döndür
                if (!string.IsNullOrWhiteSpace(createDto.PackingNo))
                {
                    var existingHeader = await _unitOfWork.PHeaders
                        .AsQueryable()
                        .FirstOrDefaultAsync(h => !h.IsDeleted && h.PackingNo == createDto.PackingNo);

                    if (existingHeader != null)
                    {
                        var errorMessage = _localizationService.GetLocalizedString("PHeaderPackingNoAlreadyExists");
                        return ApiResponse<PHeaderDto>.ErrorResult(errorMessage, errorMessage, 400);
                    }
                }

                var header = _mapper.Map<PHeader>(createDto);
                if (string.IsNullOrWhiteSpace(header.Status))
                {
                    header.Status = PHeaderStatus.Draft;
                }

                await _unitOfWork.PHeaders.AddAsync(header);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PHeaderCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto>> UpdateAsync(long id, UpdatePHeaderDto updateDto)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PHeaderDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, header);
                _unitOfWork.PHeaders.Update(header);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PHeaderUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                if (header.IsMatched)
                {
                    var errorMsg = _localizationService.GetLocalizedString("PHeaderCannotDeleteWhenMatched");
                    return ApiResponse<bool>.ErrorResult(errorMsg, errorMsg, 400);
                }

                await _unitOfWork.PHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PHeaderSoftDeletionError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> MatchPlinesWithMatchedStatus(long pHeaderId, bool isMatched)
        {
            try
            {
                // 1. Gelen datayı id ile getle ve SourceType al, eğer boş değilse
                var pHeader = await _unitOfWork.PHeaders.GetByIdAsync(pHeaderId);
                if (pHeader == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("PHeaderNotFound"),
                        _localizationService.GetLocalizedString("PHeaderNotFound"),
                        404);
                }

                if (string.IsNullOrWhiteSpace(pHeader.SourceType))
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("InvalidSourceType"),
                        _localizationService.GetLocalizedString("InvalidSourceType"),
                        400);
                }

                // 2. SourceType'a göre WT ise WtHeader, WtImportLine ama GR ise GrHeader, GrImportLine gibi olsun
                // Case when ile repo bilgisini header = GrHeader diye atayım
                object? headerRepository = null;
                object? importLineRepository = null;
                object? routeRepository = null;
                object? lineRepository = null;
                object? lineSerialsRepository = null;
                object? parameterRepository = null;


                switch (pHeader.SourceType.ToUpperInvariant())
                {
                    case PHeaderSourceType.GR:
                        headerRepository = _unitOfWork.GrHeaders;
                        importLineRepository = _unitOfWork.GrImportLines;
                        routeRepository = _unitOfWork.GrRoutes;
                        lineRepository = _unitOfWork.GrLines;
                        lineSerialsRepository = _unitOfWork.GrLineSerials;
                        parameterRepository = _unitOfWork.GrParameters;
                        break;

                    case PHeaderSourceType.WT:
                        headerRepository = _unitOfWork.WtHeaders;
                        importLineRepository = _unitOfWork.WtImportLines;
                        routeRepository = _unitOfWork.WtRoutes;
                        lineRepository = _unitOfWork.WtLines;
                        lineSerialsRepository = _unitOfWork.WtLineSerials;
                        parameterRepository = _unitOfWork.WtParameters;
                        break;

                    case PHeaderSourceType.WO:
                        headerRepository = _unitOfWork.WoHeaders;
                        importLineRepository = _unitOfWork.WoImportLines;
                        routeRepository = _unitOfWork.WoRoutes;
                        lineRepository = _unitOfWork.WoLines;
                        lineSerialsRepository = _unitOfWork.WoLineSerials;
                        parameterRepository = _unitOfWork.WoParameters;
                        break;

                    case PHeaderSourceType.WI:
                        headerRepository = _unitOfWork.WiHeaders;
                        importLineRepository = _unitOfWork.WiImportLines;
                        routeRepository = _unitOfWork.WiRoutes;
                        lineRepository = _unitOfWork.WiLines;
                        lineSerialsRepository = _unitOfWork.WiLineSerials;
                        parameterRepository = _unitOfWork.WiParameters;
                        break;

                    case PHeaderSourceType.SH:
                        headerRepository = _unitOfWork.ShHeaders;
                        importLineRepository = _unitOfWork.ShImportLines;
                        routeRepository = _unitOfWork.ShRoutes;
                        lineRepository = _unitOfWork.ShLines;
                        lineSerialsRepository = _unitOfWork.ShLineSerials;
                        parameterRepository = _unitOfWork.ShParameters;
                        break;

                    case PHeaderSourceType.PR:
                        headerRepository = _unitOfWork.PrHeaders;
                        importLineRepository = _unitOfWork.PrImportLines;
                        routeRepository = _unitOfWork.PrRoutes;
                        lineRepository = _unitOfWork.PrLines;
                        lineSerialsRepository = _unitOfWork.PrLineSerials;
                        parameterRepository = _unitOfWork.PrParameters;
                        break;

                    case PHeaderSourceType.PT:
                        headerRepository = _unitOfWork.PtHeaders;
                        importLineRepository = _unitOfWork.PtImportLines;
                        routeRepository = _unitOfWork.PtRoutes;
                        lineRepository = _unitOfWork.PtLines;
                        lineSerialsRepository = _unitOfWork.PtLineSerials;
                        parameterRepository = _unitOfWork.PtParameters;
                        break;

                    case PHeaderSourceType.SIT:
                        headerRepository = _unitOfWork.SitHeaders;
                        importLineRepository = _unitOfWork.SitImportLines;
                        routeRepository = _unitOfWork.SitRoutes;
                        lineRepository = _unitOfWork.SitLines;
                        lineSerialsRepository = _unitOfWork.SitLineSerials;
                        parameterRepository = _unitOfWork.SitParameters;
                        break;

                    case PHeaderSourceType.SRT:
                        headerRepository = _unitOfWork.SrtHeaders;
                        importLineRepository = _unitOfWork.SrtImportLines;
                        routeRepository = _unitOfWork.SrtRoutes;
                        lineRepository = _unitOfWork.SrtLines;
                        lineSerialsRepository = _unitOfWork.SrtLineSerials;
                        parameterRepository = _unitOfWork.SrtParameters;
                        break;

                    default:
                        return ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("InvalidSourceType"),
                            _localizationService.GetLocalizedString("InvalidSourceType"),
                            400);
                }

                // 3. PHeaderService.cs (224-227) ile plines var mı kontrolü
                var plines = await _unitOfWork.PLines
                    .AsQueryable()
                    .Where(pl => pl.PackingHeaderId == pHeaderId && !pl.IsDeleted)
                    .ToListAsync();

                if (plines.Count == 0)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("PLineNotFound"),
                        _localizationService.GetLocalizedString("PLineNotFound"),
                        404);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // 4. isMatched if else ile true ise ekle false ise çıkart yapacağız
                    if (isMatched)
                    {
                        // Bağlantı oluştur - Route'ları oluştur
                        if (headerRepository == null || importLineRepository == null || routeRepository == null || lineRepository == null || lineSerialsRepository == null || parameterRepository == null || pHeader.SourceHeaderId == null)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<bool>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidSourceType"),
                                _localizationService.GetLocalizedString("InvalidSourceType"),
                                400);
                        }

                        // SourceHeaderId ile header'ı kontrol et
                        if (pHeader.SourceHeaderId == null)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<bool>.ErrorResult(
                                _localizationService.GetLocalizedString("SourceHeaderIdNotFound"),
                                _localizationService.GetLocalizedString("SourceHeaderIdNotFound"),
                                400);
                        }

                        dynamic headerRepo = headerRepository;
                        dynamic? sourceHeader = null;
                        if (pHeader.SourceHeaderId.HasValue && headerRepository != null)
                        {
                            sourceHeader = await headerRepo.GetByIdAsync(pHeader.SourceHeaderId.Value);
                        }
                        if (sourceHeader == null || (sourceHeader?.IsDeleted ?? false))
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<bool>.ErrorResult(
                                _localizationService.GetLocalizedString("SourceHeaderNotFound"),
                                _localizationService.GetLocalizedString("SourceHeaderNotFound"),
                                404);
                        }

                        // Parameter'ı al
                        dynamic paramRepo = parameterRepository;
                        var queryable = paramRepo.AsQueryable();
                        var allParameters = await Task.Run(() => ((IQueryable)queryable).Cast<dynamic>().ToList());
                        dynamic? parameter = null;
                        foreach (var p in allParameters)
                        {
                            if (p != null && !(p.IsDeleted ?? false))
                            {
                                parameter = p;
                                break;
                            }
                        }

                        foreach (var pline in plines)
                        {
                            // ============================================
                            // 1) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi ile header'a ait silinmemiş Line'ları bul
                            // ============================================
                            var plineStockCode = (pline.StockCode ?? "").Trim();
                            var plineYapKod = (pline.YapKod ?? "").Trim();

                            // Line'ları StockCode ve YapKod ile eşleştir
                            dynamic lineRepo = lineRepository;
                            var lineQueryable = lineRepo.AsQueryable();
                            var allLines = await Task.Run(() => ((IQueryable)lineQueryable).Cast<dynamic>().ToList());
                            
                            // Memory'de filtreleme yap
                            var matchingLines = new List<dynamic>();
                            foreach (var line in allLines)
                            {
                                if (line != null 
                                    && line.HeaderId == pHeader.SourceHeaderId.Value
                                    && !(line.IsDeleted ?? false)
                                    && ((line.StockCode ?? "").Trim() == plineStockCode)
                                    && ((line.YapKod ?? "").Trim() == plineYapKod))
                                {
                                    matchingLines.Add(line);
                                }
                            }

                            if (matchingLines.Count == 0)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return ApiResponse<bool>.ErrorResult(
                                    _localizationService.GetLocalizedString("PLineStockCodeAndYapKodNotMatch"),
                                    $"PLine Id {pline.Id}: StockCode ({plineStockCode}) and YapKod ({plineYapKod}) do not match any Line in SourceHeader",
                                    404);
                            }

                            // ============================================
                            // 2) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                            // ============================================
                            var serialNo = (pline.SerialNo ?? "").Trim();
                            var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                            // Eşleşen Line'ların (StockCode + YapKod ile eşleşen) LineSerial'larını kontrol et
                            var lineIds = new List<long>();
                            foreach (var line in matchingLines)
                            {
                                lineIds.Add((long)line.Id);
                            }
                            dynamic lineSerialsRepo = lineSerialsRepository;
                            var lineSerialsQueryable = lineSerialsRepo.AsQueryable();
                            var allLineSerials = await Task.Run(() => ((IQueryable)lineSerialsQueryable).Cast<dynamic>().ToList());
                            
                            // Memory'de filtreleme yap
                            var lineSerials = new List<dynamic>();
                            foreach (var ls in allLineSerials)
                            {
                                if (ls != null && !(ls.IsDeleted ?? false) && lineIds.Contains(ls.LineId ?? 0))
                                {
                                    lineSerials.Add(ls);
                                }
                            }

                            // Eşleşen Line'ların LineSerial'larında SerialNo var mı kontrol et
                            var hasSerialInLineSerials = lineSerials.Any(ls => ls != null && !string.IsNullOrWhiteSpace(ls.SerialNo));

                            // ============================================
                            // DURUM 1: Her ikisinde de SerialNo var → Seri eşleşmesi + Seri bazlı miktar kontrolü
                            // ============================================
                            if (hasSerialInLineSerials && hasRequestSerial)
                            {
                                var matchingLineSerials = lineSerials.Where(ls => ls != null && ((ls.SerialNo ?? "").Trim() == serialNo)).ToList();

                                if (matchingLineSerials.Count == 0)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<bool>.ErrorResult(
                                        _localizationService.GetLocalizedString("PLineSerialNotMatch"),
                                        $"PLine Id {pline.Id}: SerialNo ({serialNo}) does not match any LineSerial",
                                        404);
                                }

                                // Seri bazlı miktar kontrolü
                                decimal totalLineSerialQuantity = 0;
                                foreach (var ls in matchingLineSerials)
                                {
                                    if (ls != null)
                                    {
                                        totalLineSerialQuantity += (decimal)(ls.Quantity ?? 0);
                                    }
                                }
                                
                                // Route'ları kontrol et
                                dynamic routeRepo = routeRepository;
                                var routeQueryable = routeRepo.AsQueryable();
                                var allRoutes = await Task.Run(() => ((IQueryable)routeQueryable).Cast<dynamic>().ToList());
                                decimal totalRouteQuantity = 0;
                                foreach (var r in allRoutes)
                                {
                                    if (r != null && !(r.IsDeleted ?? false) && r.ImportLine != null && !(r.ImportLine.IsDeleted ?? false))
                                    {
                                        var routeLineId = r.ImportLine.LineId ?? 0;
                                        if (lineIds.Contains(routeLineId) && ((r.SerialNo ?? "").Trim() == serialNo))
                                        {
                                            totalRouteQuantity += (decimal)(r.Quantity ?? 0);
                                        }
                                    }
                                }

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + pline.Quantity;
                                bool allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                                    var exceptionMessage = $"Serial {serialNo} (StockCode: {plineStockCode}, YapKod: {plineYapKod}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<bool>.ErrorResult(localizedMessage, exceptionMessage, 400);
                                }
                            }
                            // ============================================
                            // DURUM 2: LineSerial'da SerialNo yok VEYA Request'te SerialNo yok → Toplam miktar kontrolü (seri bazlı değil)
                            // ============================================
                            else
                            {
                                // Tüm LineSerial'ların toplam miktarı
                                decimal totalLineSerialQuantity = 0;
                                foreach (var ls in lineSerials)
                                {
                                    if (ls != null)
                                    {
                                        totalLineSerialQuantity += (decimal)(ls.Quantity ?? 0);
                                    }
                                }

                                // Tüm Route'ların toplam miktarı (eşleşen Line'lar için)
                                dynamic routeRepo = routeRepository;
                                var routeQueryable2 = routeRepo.AsQueryable();
                                var allRoutes = await Task.Run(() => ((IQueryable)routeQueryable2).Cast<dynamic>().ToList());
                                decimal totalRouteQuantity = 0;
                                foreach (var r in allRoutes)
                                {
                                    if (r != null && !(r.IsDeleted ?? false) && r.ImportLine != null && !(r.ImportLine.IsDeleted ?? false))
                                    {
                                        var routeLineId = r.ImportLine.LineId ?? 0;
                                        if (lineIds.Contains(routeLineId))
                                        {
                                            totalRouteQuantity += (decimal)(r.Quantity ?? 0);
                                        }
                                    }
                                }

                                // Miktar validasyonu: Sadece fazla alım kontrolü
                                var totalRouteQuantityAfterAdd = totalRouteQuantity + pline.Quantity;
                                bool allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;

                                // Eğer fazla alım izni yoksa ve Route miktarı LineSerial miktarını aşıyorsa hata
                                if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                                    var exceptionMessage = $"StockCode: {plineStockCode}, YapKod: {plineYapKod}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                    return ApiResponse<bool>.ErrorResult(localizedMessage, exceptionMessage, 400);
                                }
                            }

                            // ============================================
                            // 3) IMPORTLINE BUL/OLUŞTUR: En uygun Line'ı seç ve ImportLine bul/oluştur
                            // ============================================
                            long? selectedLineId = null;

                            // Eğer seri varsa ve sadece bir Line'da o seri varsa → O Line'ı seç
                            if (hasSerialInLineSerials && hasRequestSerial)
                            {
                                var linesWithSerial = lineSerials
                                    .Where(ls => ls != null && ((ls?.SerialNo ?? "").Trim() == serialNo))
                                    .Select(ls => (long)(ls.LineId ?? 0))
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
                                    if (line == null) continue;
                                    
                                    var lineId = (long)(line.Id ?? 0);
                                    if (lineId == 0) continue;
                                    
                                    // LineSerial toplam miktarı
                                    decimal lineSerialTotal = 0;
                                    foreach (var ls in lineSerials)
                                    {
                                        if (ls != null && (ls?.LineId ?? 0) == lineId)
                                        {
                                            lineSerialTotal += (decimal)(ls?.Quantity ?? 0);
                                        }
                                    }

                                    // Route toplam miktarı (bu Line'a bağlı ImportLine'ların Route'ları)
                                    dynamic routeRepo = routeRepository;
                                    var routeQueryable3 = routeRepo.AsQueryable();
                                    var allRoutes = await Task.Run(() => ((IQueryable)routeQueryable3).Cast<dynamic>().ToList());
                                    decimal routeTotal = 0;
                                    foreach (var r in allRoutes)
                                    {
                                        if (r != null && !(r?.IsDeleted ?? false) && r?.ImportLine != null && !(r?.ImportLine?.IsDeleted ?? false) && (r?.ImportLine?.LineId ?? 0) == lineId)
                                        {
                                            routeTotal += (decimal)(r?.Quantity ?? 0);
                                        }
                                    }

                                    var remaining = lineSerialTotal - routeTotal;
                                    lineQuantities.Add((lineId, lineSerialTotal, routeTotal, remaining));
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
                                    selectedLineId = (long)matchingLines[0].Id;
                                }
                            }

                            // Seçilen Line için ImportLine bul veya oluştur
                            if (!selectedLineId.HasValue)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return ApiResponse<bool>.ErrorResult(
                                    _localizationService.GetLocalizedString("PLineNoMatchingLine"),
                                    _localizationService.GetLocalizedString("PLineNoMatchingLine"),
                                    400);
                            }

                            // ImportLine bul veya oluştur
                            dynamic importLineRepo = importLineRepository;
                            var importLineQueryable = importLineRepo.AsQueryable();
                            var allImportLines = await Task.Run(() => ((IQueryable)importLineQueryable).Cast<dynamic>().ToList());
                            
                            // Memory'de filtreleme yap
                            dynamic? existingImportLine = null;
                            foreach (var il in allImportLines)
                            {
                                if (il != null
                                    && il.HeaderId == pHeader.SourceHeaderId.Value
                                    && il.LineId == selectedLineId.Value
                                    && ((il.StockCode ?? "").Trim() == plineStockCode)
                                    && ((il.YapKod ?? "").Trim() == plineYapKod)
                                    && !(il.IsDeleted ?? false))
                                {
                                    existingImportLine = il;
                                    break;
                                }
                            }

                            dynamic? importLine = existingImportLine;

                            if (importLine == null)
                            {
                                // ImportLine oluştur
                                dynamic newImportLine = Activator.CreateInstance(importLineRepo.GetType().GetGenericArguments()[0]);
                                newImportLine.HeaderId = pHeader.SourceHeaderId.Value;
                                newImportLine.LineId = selectedLineId.Value;
                                newImportLine.StockCode = plineStockCode;
                                newImportLine.YapKod = plineYapKod;
                                importLine = await importLineRepo.AddAsync(newImportLine);
                                await _unitOfWork.SaveChangesAsync();
                            }

                            // ============================================
                            // 4) ROUTE KAYDI: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                            // ============================================
                            dynamic routeRepoFinal = routeRepository;
                            dynamic newRoute = Activator.CreateInstance(routeRepoFinal.GetType().GetGenericArguments()[0]);
                            newRoute.ImportLineId = importLine.Id;
                            newRoute.ScannedBarcode = pline.Barcode ?? "";
                            newRoute.Quantity = pline.Quantity;
                            newRoute.SerialNo = pline.SerialNo;
                            newRoute.SerialNo2 = pline.SerialNo2;
                            newRoute.SerialNo3 = pline.SerialNo3;
                            newRoute.SerialNo4 = pline.SerialNo4;

                            var createdRoute = await routeRepoFinal.AddAsync(newRoute);
                            await _unitOfWork.SaveChangesAsync();

                            // PLine'a SourceRouteId'yi set et
                            pline.SourceRouteId = createdRoute.Id;
                            _unitOfWork.PLines.Update(pline);
                        }
                    }
                    else
                    {
                        // Bağlantıyı kes - Route'ları soft delete et
                        if (routeRepository != null)
                        {
                        dynamic headerRepo = headerRepository;
                        dynamic? sourceHeader = null;
                        if (pHeader.SourceHeaderId.HasValue && headerRepository != null)
                        {
                            sourceHeader = await headerRepo.GetByIdAsync(pHeader.SourceHeaderId.Value);
                        }
                        
                        // Source header var olmalı, silinmemiş ve tamamlanmamış olmalı
                        if (sourceHeader == null || 
                            (sourceHeader?.IsDeleted ?? false == true) || 
                            (sourceHeader?.IsCompleted ?? false == true))
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<bool>.ErrorResult(
                                _localizationService.GetLocalizedString("MatchedSourceHeaderMustBeActiveAndIncomplete"),
                                _localizationService.GetLocalizedString("MatchedSourceHeaderMustBeActiveAndIncomplete"),
                                400);
                        }

                            foreach (var pline in plines)
                            {
                                if (pline.SourceRouteId.HasValue)
                                {
                                    // Dynamic kullanarak route repository'nin SoftDelete metodunu çağır
                                    dynamic routeRepo = routeRepository;
                                    dynamic? route = await routeRepo.GetByIdAsync(pline.SourceRouteId.Value);
                                    if (route != null)
                                    {
                                        bool isDeleted = route.IsDeleted ?? false;
                                        if (!isDeleted)
                                        {
                                            await routeRepo.SoftDelete(pline.SourceRouteId.Value);
                                        }
                                    }
                                }
                                pline.SourceRouteId = null;
                                _unitOfWork.PLines.Update(pline);
                            }
                        }
                    }

                    pHeader.IsMatched = isMatched;
                    _unitOfWork.PHeaders.Update(pHeader);
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(
                        true,
                        _localizationService.GetLocalizedString("PHeaderMatchedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("PHeaderErrorOccurred"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<object>>> GetAvailableHeadersForMappingAsync(string sourceType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceType))
                {
                    return ApiResponse<IEnumerable<object>>.ErrorResult(
                        _localizationService.GetLocalizedString("InvalidSourceType"),
                        _localizationService.GetLocalizedString("InvalidSourceType"),
                        400);
                }

                // PHeader tablosunda bu SourceType ile eşleşmiş header ID'lerini al
                var mappedHeaderIds = await _unitOfWork.PHeaders
                    .AsQueryable()
                    .Where(ph => !ph.IsDeleted && ph.SourceType == sourceType && ph.SourceHeaderId.HasValue)
                    .Select(ph => ph.SourceHeaderId!.Value)
                    .ToListAsync();

                // SourceType'a göre ilgili header'ları çek ve eşleşmemiş olanları döndür
                IEnumerable<object> result = sourceType.ToUpperInvariant() switch
                {
                    PHeaderSourceType.GR => await GetAvailableGrHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WT => await GetAvailableWtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SH => await GetAvailableShHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.PR => await GetAvailablePrHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.PT => await GetAvailablePtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SIT => await GetAvailableSitHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SRT => await GetAvailableSrtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WI => await GetAvailableWiHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WO => await GetAvailableWoHeadersAsync(mappedHeaderIds),
                    _ => Array.Empty<object>()
                };

                return ApiResponse<IEnumerable<object>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<object>>.ErrorResult(
                    _localizationService.GetLocalizedString("AvailableHeadersRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        private async Task<IEnumerable<object>> GetAvailableGrHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.GrHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<GrHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableShHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.ShHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ShHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailablePrHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.PrHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PrHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailablePtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.PtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PtHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableSitHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.SitHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableSrtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.SrtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWiHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WiHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWoHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WoHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }
    }
}

