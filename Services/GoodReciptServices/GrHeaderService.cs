using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
 
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class GrHeaderService : IGrHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        private readonly IGoodReciptFunctionsService _goodReceiptFunctionsService;

        public GrHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService, IGoodReciptFunctionsService goodReceiptFunctionsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
            _goodReceiptFunctionsService = goodReceiptFunctionsService;
        }

        public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.GrHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);

                // Sorting (default by Id)
                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "PlannedDate":
                        query = desc ? query.OrderByDescending(x => x.PlannedDate) : query.OrderBy(x => x.PlannedDate);
                        break;
                    case "ERPReferenceNumber":
                        query = desc ? query.OrderByDescending(x => x.ERPReferenceNumber) : query.OrderBy(x => x.ERPReferenceNumber);
                        break;
                    case "CreatedDate":
                        query = desc ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate);
                        break;
                    default:
                        query = desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                        break;
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<GrHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<GrHeaderDto>(dtos, totalCount, pageNumber, pageSize);

                return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result,_localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var grHeaders = await _unitOfWork.GrHeaders.FindAsync(x => x.BranchCode == branchCode);
                var grHeaderDtos = _mapper.Map<List<GrHeaderDto>>(grHeaders);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(grHeaderDtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }

                var resultDtos = enrichedCustomer.Data?.ToList() ?? grHeaderDtos;
                return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(resultDtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<GrHeaderDto?>> GetByIdAsync(int id)
        {
            try
            {
                var grHeader = await _unitOfWork.GrHeaders.GetByIdAsync(id);
                if (grHeader == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<GrHeaderDto?>.ErrorResult(nf, nf, 404);
                }
                var grHeaderDto = _mapper.Map<GrHeaderDto>(grHeader);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { grHeaderDto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<GrHeaderDto?>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                grHeaderDto = enrichedCustomer.Data?.FirstOrDefault();
                return ApiResponse<GrHeaderDto?>.SuccessResult(grHeaderDto,_localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrHeaderDto?>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderRetrievalError"), ex.Message, 500, "Error retrieving GrHeader data");
            }
        }
        public async Task<ApiResponse<GrHeaderDto>> CreateAsync(CreateGrHeaderDto createDto)
        {
            try
            {
                var grHeader = _mapper.Map<GrHeader>(createDto);

                await _unitOfWork.GrHeaders.AddAsync(grHeader);
                await _unitOfWork.SaveChangesAsync();

                var grHeaderDto = _mapper.Map<GrHeaderDto>(grHeader);
                return ApiResponse<GrHeaderDto>.SuccessResult(grHeaderDto,_localizationService.GetLocalizedString("GrHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCreationError"),ex.Message,500);
            }
        }

        public async Task<ApiResponse<GrHeaderDto>> UpdateAsync(int id, UpdateGrHeaderDto updateDto)
        {
            try
            {
                var grHeader = await _unitOfWork.GrHeaders.GetByIdAsync(id);
                if (grHeader == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<GrHeaderDto>.ErrorResult(nf, nf, 404);
                }

                // Map updateDto to grHeader
                _mapper.Map(updateDto, grHeader);

                _unitOfWork.GrHeaders.Update(grHeader);
                await _unitOfWork.SaveChangesAsync();

                var grHeaderDto = _mapper.Map<GrHeaderDto>(grHeader);
                return ApiResponse<GrHeaderDto>.SuccessResult(grHeaderDto,_localizationService.GetLocalizedString("GrHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrHeaderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderUpdateError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            try
            {
                var grHeader = await _unitOfWork.GrHeaders.GetByIdAsync(id);
                if (grHeader == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                await _unitOfWork.GrHeaders.SoftDelete(grHeader.Id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrHeaderDeletedSuccessfully"));
                
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderSoftDeletionError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var grHeaders = await _unitOfWork.GrHeaders
                    .FindAsync(x => x.CustomerCode == customerCode && x.BranchCode == branchCode);
                
                var grHeaderDtos = _mapper.Map<IEnumerable<GrHeaderDto>>(grHeaders);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(grHeaderDtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                var resultDtos = enrichedCustomer.Data ?? grHeaderDtos;
                return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(resultDtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }


        public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var grHeaders = await _unitOfWork.GrHeaders
                    .FindAsync(x => x.BranchCode == branchCode);

                var grHeaderDtos = _mapper.Map<IEnumerable<GrHeaderDto>>(grHeaders);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(grHeaderDtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                var resultDtos = enrichedCustomer.Data ?? grHeaderDtos;
                return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(resultDtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var grHeaders = await _unitOfWork.GrHeaders
                    .FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && x.BranchCode == branchCode);
                
                var grHeaderDtos = _mapper.Map<IEnumerable<GrHeaderDto>>(grHeaders);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(grHeaderDtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                var resultDtos = enrichedCustomer.Data ?? grHeaderDtos;
                return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(resultDtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<long>> BulkCreateAsync(BulkCreateGrRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        // İstek ve başlık doğrulamaları
                        if (request == null || request.Header == null)
                        {
                            return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                        }

                        if (string.IsNullOrWhiteSpace(request.Header.BranchCode) || string.IsNullOrWhiteSpace(request.Header.CustomerCode))
                        {
                            return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                        }
                        
                        var header = await InsertHeaderAsync(request.Header);

                        // Başlık ekleme sonrası kontrol
                        if (header == null || header.Id <= 0)
                        {
                            return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCreateError"), _localizationService.GetLocalizedString("HeaderInsertFailed"), 500);
                        }
                        if (request.Documents != null && request.Documents.Count > 0)
                        {
                            await InsertDocumentsAsync(header.Id, request.Documents);
                        }

                        // Satır ve İçe Aktarma Satır Oluşturma
                        if (request.ImportLines == null || request.ImportLines.Count == 0)
                        {
                            request = await CreateLineAndImportLineAsync(request);
                            // FIFO tahsisi sonrası gerekli Lines/ImportLines üretildi mi?
                            if (request == null || request.Lines == null || request.Lines.Count == 0 || request.ImportLines == null || request.ImportLines.Count == 0)
                            {
                                return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("GrLineCreationError"), _localizationService.GetLocalizedString("LineGenerationFailed"), 400);
                            }
                        }

                        var lineKeyToId = request.Lines != null && request.Lines.Count > 0
                            ? await InsertLinesAsync(header.Id, request.Lines)
                            : new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

                        var insertedSerials = request.SerialLines != null && request.SerialLines.Count > 0
                            ? await InsertSerialsPreImportAsync(request.SerialLines)
                            : new List<GrLineSerial>();


                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineIdToStockCode = new Dictionary<long, string>();
                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            foreach (var importDto in request.ImportLines)
                            {
                                if (string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("LineClientKeyMissing"), 400);
                                }
                                if (!lineKeyToId.TryGetValue(importDto.LineClientKey, out var lineId))
                                {
                                    return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("LineClientKeyNotFound"), 400);
                                }
                            }
                            var result = await InsertImportLinesAsync(header.Id, request.ImportLines, lineKeyToId);
                            importLineKeyToId = result.importLineKeyToId;
                            importLineIdToStockCode = result.importLineIdToStockCode;
                        }

                        if (insertedSerials.Count > 0)
                        {
                            var err = await ResolveSerialsImportIdsAsync(insertedSerials, importLineKeyToId);
                            if (err != null) { return err; }
                        }

                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var err = await InsertRoutesAsync(request.Routes, importLineKeyToId, importLineIdToStockCode);
                            if (err != null) { return err; }
                        }

                        await tx.CommitAsync();
                        return ApiResponse<long>.SuccessResult(header.Id, _localizationService.GetLocalizedString("GrHeaderCreatedSuccessfully"));
                    }
                    catch
                    {
                        await tx.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCreationError"), ex.Message, 500);
            }
        }

        private async Task<BulkCreateGrRequestDto> CreateLineAndImportLineAsync(BulkCreateGrRequestDto request)
        {
            try
            {
                // ERP açık sipariş satırlarını müşteri ve şube koduna göre al
                var activeErpOrderLines = await _goodReceiptFunctionsService.GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(request.Header.BranchCode, request.Header.CustomerCode);
                if (activeErpOrderLines == null || activeErpOrderLines?.Success == false)
                {
                   throw new Exception(_localizationService.GetLocalizedString("GrLineCreationError"));
                }

                // ERP satırlarını (StockCode, YapKod) bazında grupla ve FIFO için OrderID'ye göre sırala
                var erpGrouped = (activeErpOrderLines.Data ?? new List<GoodsOpenOrdersLineDto>())
                    .GroupBy(x => ((x.StockCode ?? string.Empty).Trim(), (x.YapKod ?? string.Empty).Trim()));
                var erpRemainingByStockYap = erpGrouped
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.RemainingForImport ?? 0m));
                var erpInfoByStockYap = erpGrouped
                    .ToDictionary(g => g.Key, g => g.First());
                var erpListByStockYap = erpGrouped
                    .ToDictionary(g => g.Key, g => g.OrderBy(x => x.OrderID).ToList());

                var importByClientKey = (request.ImportLines ?? new List<CreateGrImportLWithLineKeyDto>())
                    .ToDictionary(i => i.ClientKey, i => i, StringComparer.OrdinalIgnoreCase);

                // Kullanıcıdan gelen import key'lerine bağlı toplam route miktarını (StockCode, YapKod) bazında topla
                var importlinesRemainingByStockYap = (request.Routes ?? new List<CreateGrRouteWithImportLineKeyDto>())
                    .Where(r => importByClientKey.ContainsKey(r.ImportLineClientKey))
                    .Select(r => new { Route = r, Import = importByClientKey[r.ImportLineClientKey] })
                    .GroupBy(x => ((x.Import.StockCode ?? string.Empty).Trim(), (x.Import.YapKod ?? string.Empty).Trim()))
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Route.Quantity));

                foreach (var kvp in importlinesRemainingByStockYap)
                {
                    var key = kvp.Key;
                    var plannedQty = kvp.Value;
                    if (!erpRemainingByStockYap.TryGetValue(key, out var remainingQty))
                    {
                        throw new Exception(_localizationService.GetLocalizedString("ErpRemainingNotFound") + $": StockCode={key.Item1}, YapKod={key.Item2}");
                    }
                    if (plannedQty > remainingQty)
                    {
                        throw new Exception(_localizationService.GetLocalizedString("GrRouteExceedsErpRemaining") + $": StockCode={key.Item1}, YapKod={key.Item2}, Planned={plannedQty}, RemainingForImport={remainingQty}");
                    }
                }

                var importSampleByStockYap = (request.ImportLines ?? new List<CreateGrImportLWithLineKeyDto>())
                    .GroupBy(x => ((x.StockCode ?? string.Empty).Trim(), (x.YapKod ?? string.Empty).Trim()))
                    .ToDictionary(g => g.Key, g => g.First());

                // FIFO tahsisi sonrası oluşturulacak geçici istek (lines/importLines/routes)
                var tempRequest = new BulkCreateGrRequestDto
                {
                    Header = request.Header,
                    Documents = request.Documents,
                    Lines = new List<CreateGrLineWithKeyDto>(),
                    ImportLines = new List<CreateGrImportLWithLineKeyDto>(),
                    SerialLines = request.SerialLines ?? new List<CreateGrImportSerialLineWithImportLineKeyDto>(),
                    Routes = new List<CreateGrRouteWithImportLineKeyDto>()
                };

                var originalImportKeyToNewImportKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                var groupedRoutes = (request.Routes ?? new List<CreateGrRouteWithImportLineKeyDto>())
                    .Where(r => importByClientKey.ContainsKey(r.ImportLineClientKey))
                    .Select(r => new { Route = r, Import = importByClientKey[r.ImportLineClientKey] })
                    .GroupBy(x => ((x.Import.StockCode ?? string.Empty).Trim(), (x.Import.YapKod ?? string.Empty).Trim()));

                // Her (StockCode, YapKod) grubu için FIFO tahsisini uygula
                foreach (var grp in groupedRoutes)
                {
                    var key = grp.Key;
                    var sample = importSampleByStockYap.TryGetValue(key, out var s) ? s : null;
                    var erpList = erpListByStockYap.TryGetValue(key, out var list) ? list : new List<GoodsOpenOrdersLineDto>();
                    var routeQueue = new Queue<(CreateGrRouteWithImportLineKeyDto Route, string OriginalImportKey, decimal Qty)>();
                    foreach (var x in grp)
                    {
                        // Route'ları sıraya ekle; her bir route miktarı tahsis edilecek ve gerekirse bölünecek
                        routeQueue.Enqueue((x.Route, x.Import.ClientKey, x.Route.Quantity));
                    }

                    foreach (var erp in erpList)
                    {
                        var erpRemain = erp.RemainingForImport ?? 0m;
                        var toAllocate = erpRemain;
                        if (toAllocate <= 0) { continue; }

                        var lineKey = Guid.NewGuid().ToString("N");
                        var importKey = Guid.NewGuid().ToString("N");

                        // ERP kalan miktarı kadar yeni GR line oluştur
                        tempRequest.Lines.Add(new CreateGrLineWithKeyDto
                        {
                            ClientKey = lineKey,
                            StockCode = key.Item1,
                            Quantity = toAllocate,
                            Unit = sample?.Unit,
                            ErpOrderNo = erp.SiparisNo,
                            ErpOrderId = erp.OrderID.ToString(),
                            Description = sample?.Description1 ?? erp.StockName
                        });

                        // Line ile ilişkili yeni import satırı oluştur
                        tempRequest.ImportLines.Add(new CreateGrImportLWithLineKeyDto
                        {
                            LineClientKey = lineKey,
                            ClientKey = importKey,
                            StockCode = key.Item1,
                            YapKod = key.Item2,
                            Unit = sample?.Unit,
                            Description1 = sample?.Description1,
                            Description2 = sample?.Description2
                        });

                        // Route miktarlarını yeni import key'e FIFO sırayla parçalayarak tahsis et
                        while (toAllocate > 0 && routeQueue.Count > 0)
                        {
                            var (route, originalImportKey, qty) = routeQueue.Peek();
                            var assign = Math.Min(qty, toAllocate);
                            tempRequest.Routes.Add(new CreateGrRouteWithImportLineKeyDto
                            {
                                ImportLineClientKey = importKey,
                                ScannedBarcode = route.ScannedBarcode,
                                Quantity = assign,
                                StockCode = route.StockCode,
                                YapKod = route.YapKod,
                                Description = route.Description,
                                SerialNo = route.SerialNo,
                                SerialNo2 = route.SerialNo2,
                                SerialNo3 = route.SerialNo3,
                                SerialNo4 = route.SerialNo4,
                                SourceWarehouse = route.SourceWarehouse,
                                TargetWarehouse = route.TargetWarehouse,
                                SourceCellCode = route.SourceCellCode,
                                TargetCellCode = route.TargetCellCode
                            });

                            // Orijinal import key'i yeni import key'e bağla (seri satır remap için)
                            if (!originalImportKeyToNewImportKey.ContainsKey(originalImportKey) && !string.IsNullOrWhiteSpace(originalImportKey))
                            {
                                originalImportKeyToNewImportKey[originalImportKey] = importKey;
                            }

                            qty -= assign;
                            toAllocate -= assign;
                            if (qty <= 0)
                            {
                                // Bu route tamamen tüketildi
                                routeQueue.Dequeue();
                            }
                            else
                            {
                                // Bu route kısmen tüketildi; kalan miktar için sıranın sonuna ekle
                                routeQueue.Dequeue();
                                routeQueue.Enqueue((route, originalImportKey, qty));
                            }
                        }
                    }
                }

                // Seri satırları yeni import key eşlemelerine göre güncelle
                if (tempRequest.SerialLines != null && tempRequest.SerialLines.Count > 0)
                {
                    foreach (var s in tempRequest.SerialLines)
                    {
                        if (!string.IsNullOrWhiteSpace(s.ImportLineClientKey) && originalImportKeyToNewImportKey.TryGetValue(s.ImportLineClientKey, out var newKey))
                        {
                            s.ImportLineClientKey = newKey;
                        }
                    }
                }

                return tempRequest;
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrHeaderCreationError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task<GrHeader> InsertHeaderAsync(CreateGrHeaderDto headerDto)
        {
            try
            {
                if (headerDto == null)
                {
                    throw new Exception(_localizationService.GetLocalizedString("InvalidModelState"));
                }
                if (string.IsNullOrWhiteSpace(headerDto.BranchCode) || string.IsNullOrWhiteSpace(headerDto.CustomerCode))
                {
                    throw new Exception(_localizationService.GetLocalizedString("InvalidModelState"));
                }
                var entity = _mapper.Map<GrHeader>(headerDto);
                await _unitOfWork.GrHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrHeaderCreationError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task InsertDocumentsAsync(long headerId, List<CreateGrImportDocumentSimpleDto> docs)
        {
            try
            {
                if (docs == null || docs.Count == 0)
                {
                    return;
                }
                var entities = new List<GrImportDocument>(docs.Count);
                foreach (var d in docs)
                {
                    if (d == null || d.Base64 == null)
                    {
                        throw new Exception(_localizationService.GetLocalizedString("InvalidModelState"));
                    }
                    entities.Add(new GrImportDocument { HeaderId = headerId, Base64 = d.Base64 });
                }
                await _unitOfWork.GrImportDocuments.AddRangeAsync(entities);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrHeaderCreationError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task<Dictionary<string, long>> InsertLinesAsync(long headerId, List<CreateGrLineWithKeyDto> linesDto)
        {
            try
            {
                var lines = new List<GrLine>(linesDto.Count);
                foreach (var l in linesDto)
                {
                    lines.Add(new GrLine
                    {
                        HeaderId = headerId,
                        StockCode = l.StockCode,
                        Quantity = l.Quantity,
                        Unit = l.Unit,
                        ErpOrderNo = l.ErpOrderNo,
                        ErpOrderId = l.ErpOrderId,
                        Description = l.Description
                    });
                }
                await _unitOfWork.GrLines.AddRangeAsync(lines);
                await _unitOfWork.SaveChangesAsync();
                var map = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < linesDto.Count; i++)
                {
                    var key = linesDto[i].ClientKey;
                    if (!string.IsNullOrWhiteSpace(key)) { map[key] = lines[i].Id; }
                }
                return map;
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrLineCreateError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task<List<GrLineSerial>> InsertSerialsPreImportAsync(List<CreateGrImportSerialLineWithImportLineKeyDto> serialsDto)
        {
            try
            {
                var serials = new List<GrLineSerial>(serialsDto.Count);
                foreach (var s in serialsDto)
                {
                    serials.Add(new GrLineSerial
                    {
                        ImportLineId = null,
                        ClientKey = s.ImportLineClientKey,
                        Quantity = s.Quantity,
                        SerialNo = s.SerialNo,
                        SerialNo2 = s.SerialNo2,
                        SerialNo3 = s.SerialNo3,
                        SerialNo4 = s.SerialNo4,
                        SourceCellCode = s.SourceCellCode,
                        TargetCellCode = s.TargetCellCode,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                await _unitOfWork.GrLineSerials.AddRangeAsync(serials);
                await _unitOfWork.SaveChangesAsync();
                return serials;
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrHeaderCreationError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task<(Dictionary<string, long> importLineKeyToId, Dictionary<long, string> importLineIdToStockCode)> InsertImportLinesAsync(
            long headerId,
            List<CreateGrImportLWithLineKeyDto> importLinesDto,
            Dictionary<string, long> lineKeyToId)
        {
            try
            {
                if (importLinesDto == null || importLinesDto.Count == 0)
                {
                    return (new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase), new Dictionary<long, string>());
                }
                var importLines = new List<GrImportLine>(importLinesDto.Count);
                foreach (var iDto in importLinesDto)
                {
                    if (string.IsNullOrWhiteSpace(iDto.LineClientKey) || !lineKeyToId.ContainsKey(iDto.LineClientKey))
                    {
                        throw new Exception(_localizationService.GetLocalizedString("InvalidModelState"));
                    }
                    var lineId = lineKeyToId[iDto.LineClientKey];
                    importLines.Add(new GrImportLine
                    {
                        HeaderId = headerId,
                        LineId = lineId,
                        StockCode = iDto.StockCode,
                        Description1 = iDto.Description1,
                        Description2 = iDto.Description2
                    });
                }
                await _unitOfWork.GrImportLines.AddRangeAsync(importLines);
                await _unitOfWork.SaveChangesAsync();
                var keyMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                var stockMap = new Dictionary<long, string>();
                for (int i = 0; i < importLinesDto.Count; i++)
                {
                    var key = importLinesDto[i].ClientKey;
                    stockMap[importLines[i].Id] = importLines[i].StockCode;
                    if (!string.IsNullOrWhiteSpace(key)) { keyMap[key] = importLines[i].Id; }
                }
                return (keyMap, stockMap);
            }
            catch (Exception ex)
            {
                throw new Exception(_localizationService.GetLocalizedString("GrHeaderCreationError") + ": " + (ex.Message ?? string.Empty));
            }
        }

        private async Task<ApiResponse<long>?> ResolveSerialsImportIdsAsync(List<GrLineSerial> serials, Dictionary<string, long> importLineKeyToId)
        {
            try
            {
                foreach (var s in serials)
                {
                    if (string.IsNullOrWhiteSpace(s.ClientKey))
                    {
                        return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("ImportLineClientKeyMissing"), 400);
                    }
                    if (!importLineKeyToId.TryGetValue(s.ClientKey, out var impId))
                    {
                        return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("ImportLineClientKeyNotFound"), 400);
                    }
                    s.ImportLineId = impId;
                    _unitOfWork.GrLineSerials.Update(s);
                }
                await _unitOfWork.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        private async Task<ApiResponse<long>?> InsertRoutesAsync(List<CreateGrRouteWithImportLineKeyDto> routesDto, Dictionary<string, long> importLineKeyToId, Dictionary<long, string> importLineIdToStockCode)
        {
            try
            {
                var routes = new List<GrRoute>(routesDto.Count);
                foreach (var r in routesDto)
                {
                    if (string.IsNullOrWhiteSpace(r.ImportLineClientKey))
                    {
                        return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("ImportLineClientKeyMissing"), 400);
                    }
                    if (!importLineKeyToId.TryGetValue(r.ImportLineClientKey, out var importLineId))
                    {
                        return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidCorrelationKey"), _localizationService.GetLocalizedString("ImportLineClientKeyNotFound"), 400);
                    }
                    routes.Add(new GrRoute
                    {
                        ImportLineId = importLineId,
                        ScannedBarcode = r.ScannedBarcode,
                        Quantity = r.Quantity,
                        Description = r.Description,
                        SerialNo = r.SerialNo,
                        SerialNo2 = r.SerialNo2,
                        SerialNo3 = r.SerialNo3,
                        SerialNo4 = r.SerialNo4,
                        SourceWarehouse = r.SourceWarehouse,
                        TargetWarehouse = r.TargetWarehouse,
                        SourceCellCode = r.SourceCellCode,
                        TargetCellCode = r.TargetCellCode,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                await _unitOfWork.GrRoutes.AddRangeAsync(routes);
                await _unitOfWork.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.GrHeaders.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                entity.IsPendingApproval = false;

                _unitOfWork.GrHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderCompletionError"),
                    ex.Message,
                    500);
            }
        }

         
    }
}
