using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PLineService : IPLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetAllAsync()
        {
            try
            {
                var lines = await _unitOfWork.PLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PLines.AsQueryable();
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<PLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                
                var result = new PagedResponse<PLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto?>> GetByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<PLineDto?>.ErrorResult(nf, nf, 404);
                }

                var dto = _mapper.Map<PLineDto>(line);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PLineDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto?>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackageIdAsync(long packageId)
        {
            try
            {
                var lines = await _unitOfWork.PLines.FindAsync(x => x.PackageId == packageId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackingHeaderIdAsync(long packingHeaderId)
        {
            try
            {
                var lines = await _unitOfWork.PLines.FindAsync(x => x.PackingHeaderId == packingHeaderId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto>> CreateAsync(CreatePLineDto createDto)
        {
            try
            {
                // Validate PackingHeader exists
                var header = await _unitOfWork.PHeaders.GetByIdAsync(createDto.PackingHeaderId);
                if (header == null || header.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                // Validate Package exists
                var package = await _unitOfWork.PPackages.GetByIdAsync(createDto.PackageId);
                if (package == null || package.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PPackageNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                // Validate Package belongs to PackingHeader
                if (package.PackingHeaderId != createDto.PackingHeaderId)
                {
                    var error = _localizationService.GetLocalizedString("PPackageNotBelongToPHeader");
                    return ApiResponse<PLineDto>.ErrorResult(error, error, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var line = _mapper.Map<PLine>(createDto);

                    // Eğer PHeader IsMatched true ise, Route oluşturma işlemlerini yap
                    if (header.IsMatched && !string.IsNullOrWhiteSpace(header.SourceType) && header.SourceHeaderId.HasValue)
                    {
                        // SourceType'a göre repository'leri belirle
                        object? headerRepository = null;
                        object? importLineRepository = null;
                        object? routeRepository = null;
                        object? lineRepository = null;
                        object? lineSerialsRepository = null;
                        object? parameterRepository = null;

                        switch (header.SourceType.ToUpperInvariant())
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
                                await tx.RollbackAsync();
                                return ApiResponse<PLineDto>.ErrorResult(
                                    _localizationService.GetLocalizedString("InvalidSourceType"),
                                    _localizationService.GetLocalizedString("InvalidSourceType"),
                                    400);
                        }

                        if (headerRepository == null || importLineRepository == null || routeRepository == null || 
                            lineRepository == null || lineSerialsRepository == null || parameterRepository == null)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<PLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidSourceType"),
                                _localizationService.GetLocalizedString("InvalidSourceType"),
                                400);
                        }

                        // SourceHeader kontrolü
                        dynamic headerRepo = headerRepository;
                        dynamic? sourceHeader = await headerRepo.GetByIdAsync(header.SourceHeaderId.Value);
                        
                        if (sourceHeader == null || (sourceHeader?.IsDeleted ?? false) || (sourceHeader?.IsCompleted ?? false))
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<PLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("MatchedSourceHeaderMustBeActiveAndIncomplete"),
                                _localizationService.GetLocalizedString("MatchedSourceHeaderMustBeActiveAndIncomplete"),
                                400);
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

                        // ============================================
                        // 1) LINE UYUMLULUĞU: StokKodu ve YapKod eşleşmesi
                        // ============================================
                        var lineStockCode = (line.StockCode ?? "").Trim();
                        var lineYapKod = (line.YapKod ?? "").Trim();

                        dynamic lineRepo = lineRepository;
                        var lineQueryable = lineRepo.AsQueryable();
                        var allLines = await Task.Run(() => ((IQueryable)lineQueryable).Cast<dynamic>().ToList());
                        
                        var matchingLines = new List<dynamic>();
                        foreach (var l in allLines)
                        {
                            if (l != null 
                                && l.HeaderId == header.SourceHeaderId.Value
                                && !(l.IsDeleted ?? false)
                                && ((l.StockCode ?? "").Trim() == lineStockCode)
                                && ((l.YapKod ?? "").Trim() == lineYapKod))
                            {
                                matchingLines.Add(l);
                            }
                        }

                        if (matchingLines.Count == 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<PLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("PLineStockCodeAndYapKodNotMatch"),
                                $"StockCode ({lineStockCode}) and YapKod ({lineYapKod}) do not match any Line in SourceHeader",
                                404);
                        }

                        // ============================================
                        // 2) SERİ KONTROLÜ VE MİKTAR VALİDASYONU
                        // ============================================
                        var serialNo = (line.SerialNo ?? "").Trim();
                        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);

                        var lineIds = new List<long>();
                        foreach (var l in matchingLines)
                        {
                            lineIds.Add((long)l.Id);
                        }

                        dynamic lineSerialsRepo = lineSerialsRepository;
                        var lineSerialsQueryable = lineSerialsRepo.AsQueryable();
                        var allLineSerials = await Task.Run(() => ((IQueryable)lineSerialsQueryable).Cast<dynamic>().ToList());
                        
                        var lineSerials = new List<dynamic>();
                        foreach (var ls in allLineSerials)
                        {
                            if (ls != null && !(ls.IsDeleted ?? false) && lineIds.Contains(ls.LineId ?? 0))
                            {
                                lineSerials.Add(ls);
                            }
                        }

                        var hasSerialInLineSerials = lineSerials.Any(ls => ls != null && !string.IsNullOrWhiteSpace(ls.SerialNo));

                        // DURUM 1: Her ikisinde de SerialNo var
                        if (hasSerialInLineSerials && hasRequestSerial)
                        {
                            var matchingLineSerials = lineSerials.Where(ls => ls != null && ((ls.SerialNo ?? "").Trim() == serialNo)).ToList();

                            if (matchingLineSerials.Count == 0)
                            {
                                await tx.RollbackAsync();
                                return ApiResponse<PLineDto>.ErrorResult(
                                    _localizationService.GetLocalizedString("PLineSerialNotMatch"),
                                    $"SerialNo ({serialNo}) does not match any LineSerial",
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

                            var totalRouteQuantityAfterAdd = totalRouteQuantity + line.Quantity;
                            bool allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;

                            if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                            {
                                await tx.RollbackAsync();
                                var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                                var exceptionMessage = $"Serial {serialNo} (StockCode: {lineStockCode}, YapKod: {lineYapKod}): Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                return ApiResponse<PLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
                            }
                        }
                        // DURUM 2: LineSerial'da SerialNo yok VEYA Request'te SerialNo yok
                        else
                        {
                            decimal totalLineSerialQuantity = 0;
                            foreach (var ls in lineSerials)
                            {
                                if (ls != null)
                                {
                                    totalLineSerialQuantity += (decimal)(ls.Quantity ?? 0);
                                }
                            }

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

                            var totalRouteQuantityAfterAdd = totalRouteQuantity + line.Quantity;
                            bool allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;

                            if (!allowMore && totalRouteQuantityAfterAdd > totalLineSerialQuantity + 0.000001m)
                            {
                                await tx.RollbackAsync();
                                var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                                var exceptionMessage = $"StockCode: {lineStockCode}, YapKod: {lineYapKod}: Route total after add ({totalRouteQuantityAfterAdd}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                                return ApiResponse<PLineDto>.ErrorResult(localizedMessage, exceptionMessage, 400);
                            }
                        }

                        // ============================================
                        // 3) IMPORTLINE BUL/OLUŞTUR
                        // ============================================
                        long? selectedLineId = null;

                        if (hasSerialInLineSerials && hasRequestSerial)
                        {
                            var linesWithSerial = lineSerials
                                .Where(ls => ls != null && ((ls?.SerialNo ?? "").Trim() == serialNo))
                                .Select(ls => (long)(ls.LineId ?? 0))
                                .Distinct()
                                .ToList();

                            if (linesWithSerial.Count == 1)
                            {
                                selectedLineId = linesWithSerial.First();
                            }
                        }

                        if (!selectedLineId.HasValue)
                        {
                            var lineQuantities = new List<(long LineId, decimal LineSerialTotal, decimal RouteTotal, decimal Remaining)>();

                            foreach (var l in matchingLines)
                            {
                                if (l == null) continue;
                                
                                var lineId = (long)(l.Id ?? 0);
                                if (lineId == 0) continue;
                                
                                decimal lineSerialTotal = 0;
                                foreach (var ls in lineSerials)
                                {
                                    if (ls != null && (ls?.LineId ?? 0) == lineId)
                                    {
                                        lineSerialTotal += (decimal)(ls?.Quantity ?? 0);
                                    }
                                }

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

                            var bestLine = lineQuantities
                                .OrderByDescending(x => x.Remaining)
                                .FirstOrDefault();

                            if (bestLine.LineId > 0)
                            {
                                selectedLineId = bestLine.LineId;
                            }
                            else
                            {
                                selectedLineId = (long)matchingLines[0].Id;
                            }
                        }

                        if (!selectedLineId.HasValue)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<PLineDto>.ErrorResult(
                                _localizationService.GetLocalizedString("PLineNoMatchingLine"),
                                _localizationService.GetLocalizedString("PLineNoMatchingLine"),
                                400);
                        }

                        // ImportLine bul veya oluştur
                        dynamic importLineRepo = importLineRepository;
                        var importLineQueryable = importLineRepo.AsQueryable();
                        var allImportLines = await Task.Run(() => ((IQueryable)importLineQueryable).Cast<dynamic>().ToList());
                        
                        dynamic? existingImportLine = null;
                        foreach (var il in allImportLines)
                        {
                            if (il != null
                                && il.HeaderId == header.SourceHeaderId.Value
                                && il.LineId == selectedLineId.Value
                                && ((il.StockCode ?? "").Trim() == lineStockCode)
                                && ((il.YapKod ?? "").Trim() == lineYapKod)
                                && !(il.IsDeleted ?? false))
                            {
                                existingImportLine = il;
                                break;
                            }
                        }

                        dynamic? importLine = existingImportLine;

                        if (importLine == null)
                        {
                            dynamic newImportLine = Activator.CreateInstance(importLineRepo.GetType().GetGenericArguments()[0]);
                            newImportLine.HeaderId = header.SourceHeaderId.Value;
                            newImportLine.LineId = selectedLineId.Value;
                            newImportLine.StockCode = lineStockCode;
                            newImportLine.YapKod = lineYapKod;
                            importLine = await importLineRepo.AddAsync(newImportLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 4) ROUTE KAYDI
                        // ============================================
                        dynamic routeRepoFinal = routeRepository;
                        dynamic newRoute = Activator.CreateInstance(routeRepoFinal.GetType().GetGenericArguments()[0]);
                        newRoute.ImportLineId = importLine.Id;
                        newRoute.ScannedBarcode = line.Barcode ?? "";
                        newRoute.Quantity = line.Quantity;
                        newRoute.SerialNo = line.SerialNo;
                        newRoute.SerialNo2 = line.SerialNo2;
                        newRoute.SerialNo3 = line.SerialNo3;
                        newRoute.SerialNo4 = line.SerialNo4;

                        var createdRoute = await routeRepoFinal.AddAsync(newRoute);
                        await _unitOfWork.SaveChangesAsync();

                        // PLine'a SourceRouteId'yi set et
                        line.SourceRouteId = createdRoute.Id;
                    }

                    await _unitOfWork.PLines.AddAsync(line);
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    var dto = _mapper.Map<PLineDto>(line);
                    var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                    if (!enriched.Success)
                    {
                        // Transaction zaten commit edildi, sadece ERP hatası döndür
                        return ApiResponse<PLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                    }
                    dto = enriched.Data?.FirstOrDefault() ?? dto;
                    
                    return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineCreatedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto>.ErrorResult(_localizationService.GetLocalizedString("PLineCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto>> UpdateAsync(long id, UpdatePLineDto updateDto)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, line);
                _unitOfWork.PLines.Update(line);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PLineDto>(line);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto>.ErrorResult(_localizationService.GetLocalizedString("PLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                // PHeader kontrolü - SourceType ve SourceRouteId kontrolü
                var packingHeader = await _unitOfWork.PHeaders.GetByIdAsync(line.PackingHeaderId);
                bool shouldRetireRoute = false;
                string? sourceType = null;
                long? routeIdToRetire = null;

                if (packingHeader != null && !string.IsNullOrWhiteSpace(packingHeader.SourceType) && line.SourceRouteId.HasValue)
                {
                    shouldRetireRoute = true;
                    sourceType = packingHeader.SourceType;
                    routeIdToRetire = line.SourceRouteId.Value;
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // SourceType'a göre ilgili Route'u retired (soft delete) et
                    if (shouldRetireRoute && routeIdToRetire.HasValue && !string.IsNullOrWhiteSpace(sourceType))
                    {
                        switch (sourceType.ToUpperInvariant())
                        {
                            case PHeaderSourceType.GR:
                                var grRoute = await _unitOfWork.GrRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (grRoute != null && !grRoute.IsDeleted)
                                {
                                    await _unitOfWork.GrRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WT:
                                var wtRoute = await _unitOfWork.WtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (wtRoute != null && !wtRoute.IsDeleted)
                                {
                                    await _unitOfWork.WtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WO:
                                var woRoute = await _unitOfWork.WoRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (woRoute != null && !woRoute.IsDeleted)
                                {
                                    await _unitOfWork.WoRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WI:
                                var wiRoute = await _unitOfWork.WiRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (wiRoute != null && !wiRoute.IsDeleted)
                                {
                                    await _unitOfWork.WiRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SH:
                                var shRoute = await _unitOfWork.ShRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (shRoute != null && !shRoute.IsDeleted)
                                {
                                    await _unitOfWork.ShRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.PR:
                                var prRoute = await _unitOfWork.PrRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (prRoute != null && !prRoute.IsDeleted)
                                {
                                    await _unitOfWork.PrRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.PT:
                                var ptRoute = await _unitOfWork.PtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (ptRoute != null && !ptRoute.IsDeleted)
                                {
                                    await _unitOfWork.PtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SIT:
                                var sitRoute = await _unitOfWork.SitRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (sitRoute != null && !sitRoute.IsDeleted)
                                {
                                    await _unitOfWork.SitRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SRT:
                                var srtRoute = await _unitOfWork.SrtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (srtRoute != null && !srtRoute.IsDeleted)
                                {
                                    await _unitOfWork.SrtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            default:
                                // Bilinmeyen SourceType için işlem yapılmaz
                                break;
                        }
                    }

                    // PLine'ı sil
                    await _unitOfWork.PLines.SoftDelete(id);
                    
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PLineSoftDeletionError"), ex.Message, 500);
            }
        }

    }
}

