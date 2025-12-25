using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Security.Claims;

namespace WMS_WEBAPI.Services
{
    public class ShHeaderService : IShHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public ShHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShHeaders.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShHeaderDto>>(entities);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;

                return ApiResponse<IEnumerable<ShHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<ShHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<ShHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<ShHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> CreateAsync(CreateShHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WMS_WEBAPI.Models.ShHeader>(createDto);
                await _unitOfWork.ShHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> UpdateAsync(long id, UpdateShHeaderDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<ShHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                var importLines = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("ShHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                existing.IsDeleted = true;
                _unitOfWork.ShHeaders.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                // ============================================
                // CHECK ERP APPROVAL REQUIREMENT
                // ============================================
                var shParameter = await _unitOfWork.ShParameters
                    .AsQueryable()
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync();

                // ============================================
                // VALIDATE LINE SERIAL VS ROUTE QUANTITIES
                // ============================================
                // Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
                // Normalize null values to false
                bool skipValidation = (shParameter?.AllowLessQuantityBasedOnOrder ?? false) 
                    && (shParameter?.AllowMoreQuantityBasedOnOrder ?? false);

                // Normalize RequireAllOrderItemsCollected
                bool requireAllOrderItemsCollected = shParameter?.RequireAllOrderItemsCollected ?? false;

                if (!skipValidation)
                {
                    var lines = await _unitOfWork.ShLines
                        .AsQueryable()
                        .Where(l => l.HeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var line in lines)
                    {
                        // Get total quantity of LineSerials for this Line
                        var totalLineSerialQuantity = await _unitOfWork.ShLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId == line.Id)
                            .SumAsync(ls => ls.Quantity);

                        // Get total quantity of Routes for ImportLines linked to this Line
                        var totalRouteQuantity = await _unitOfWork.ShRoutes
                            .AsQueryable()
                            .Where(r => !r.IsDeleted 
                                && r.ImportLine.LineId == line.Id 
                                && !r.ImportLine.IsDeleted)
                            .SumAsync(r => r.Quantity);

                        // ============================================
                        // CHECK IF ALL ORDER ITEMS MUST BE COLLECTED
                        // ============================================
                        if (requireAllOrderItemsCollected)
                        {
                            // If RequireAllOrderItemsCollected is true, every line must have routes (totalRouteQuantity > 0)
                            if (totalRouteQuantity <= 0.000001m)
                            {
                                var msg = _localizationService.GetLocalizedString("ShHeaderAllOrderItemsMustBeCollected", 
                                    line.Id, 
                                    line.StockCode ?? string.Empty, 
                                    line.YapKod ?? string.Empty);
                                return ApiResponse<bool>.ErrorResult(msg, 
                                    $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): All order items must be collected. Route quantity is 0.", 
                                    400);
                            }
                        }
                        else
                        {
                            // If RequireAllOrderItemsCollected is false, skip validation for lines with no routes
                            if (totalRouteQuantity <= 0.000001m)
                            {
                                continue; // Skip this line, no routes means it's optional
                            }
                        }

                        // Determine validation logic based on parameters
                        // Normalize null values to false
                        bool allowLess = shParameter?.AllowLessQuantityBasedOnOrder ?? false;
                        bool allowMore = shParameter?.AllowMoreQuantityBasedOnOrder ?? false;
                        
                        bool quantityMismatch = false;
                        string localizedMessage = string.Empty;
                        string exceptionMessage = string.Empty;

                        if (!allowLess && !allowMore)
                        {
                            // Both false: Exact match required (==)
                            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("ShHeaderQuantityExactMatchRequired", 
                                    line.Id, 
                                    line.StockCode ?? string.Empty, 
                                    line.YapKod ?? string.Empty, 
                                    totalLineSerialQuantity, 
                                    totalRouteQuantity);
                                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): LineSerial total ({totalLineSerialQuantity}) must exactly match Route total ({totalRouteQuantity})";
                            }
                        }
                        else if (allowLess && !allowMore)
                        {
                            // AllowLessQuantityBasedOnOrder: true, AllowMoreQuantityBasedOnOrder: false
                            // Route <= LineSerial (Route can be less or equal to LineSerial)
                            // Error if Route > LineSerial
                            if (totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("ShHeaderQuantityCannotBeGreater", 
                                    line.Id, 
                                    line.StockCode ?? string.Empty, 
                                    line.YapKod ?? string.Empty, 
                                    totalLineSerialQuantity, 
                                    totalRouteQuantity);
                                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): Route total ({totalRouteQuantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                            }
                        }
                        else if (!allowLess && allowMore)
                        {
                            // AllowLessQuantityBasedOnOrder: false, AllowMoreQuantityBasedOnOrder: true
                            // Route >= LineSerial (Route can be more or equal to LineSerial)
                            // Error if Route < LineSerial
                            if (totalRouteQuantity + 0.000001m < totalLineSerialQuantity)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("ShHeaderQuantityCannotBeLess", 
                                    line.Id, 
                                    line.StockCode ?? string.Empty, 
                                    line.YapKod ?? string.Empty, 
                                    totalLineSerialQuantity, 
                                    totalRouteQuantity);
                                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): Route total ({totalRouteQuantity}) cannot be less than LineSerial total ({totalLineSerialQuantity})";
                            }
                        }

                        if (quantityMismatch)
                        {
                            return ApiResponse<bool>.ErrorResult(localizedMessage, exceptionMessage, 400);
                        }
                    }
                }

                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                
                // Set IsPendingApproval based on parameter requirement
                entity.IsPendingApproval = shParameter != null && shParameter.RequireApprovalBeforeErp;

                _unitOfWork.ShHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.ShHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.ShTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<ShHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<ShHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderAssignedOrdersRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.ShLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<ShLineSerial> lineSerials = Array.Empty<ShLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.ShLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.ShImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<ShRoute> routes = Array.Empty<ShRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.ShRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var lineDtos = _mapper.Map<IEnumerable<ShLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var importLineDtos = _mapper.Map<IEnumerable<ShImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var dto = new ShAssignedOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<ShLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<ShRouteDto>>(routes)
                };

                return ApiResponse<ShAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<ShHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                var entity = await _unitOfWork.ShHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShHeaderNotFound");
                    return ApiResponse<ShHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("ShHeaderApprovalUpdateError");
                    return ApiResponse<ShHeaderDto>.ErrorResult(msg, msg, 400);
                }

                var httpUser = _httpContextAccessor.HttpContext?.User;
                long? approvedByUserId = null;
                var claimVal = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (long.TryParse(claimVal, out var uid))
                {
                    approvedByUserId = uid;
                }

                entity.ApprovalStatus = approved;
                entity.ApprovedByUserId = approvedByUserId;
                entity.ApprovalDate = DateTime.Now;
                entity.IsPendingApproval = false;

                _unitOfWork.ShHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<ShHeaderDto>(entity);
                return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderApprovalUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ShHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.ShHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<ShHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<ShHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<ShHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<ShHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<ShHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("ShHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShHeaderDto>> GenerateShipmentOrderAsync(GenerateShipmentOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<ShHeader>(request.Header);
                        await _unitOfWork.ShHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<ShLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = _mapper.Map<ShLine>(l);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }
                            await _unitOfWork.ShLines.AddRangeAsync(lines);
                            await _unitOfWork.SaveChangesAsync();

                            for (int i = 0; i < request.Lines.Count; i++)
                            {
                                var key = request.Lines[i].ClientKey;
                                var guid = request.Lines[i].ClientGuid;
                                var id = lines[i].Id;
                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    lineKeyToId[key!] = id;
                                }
                                if (guid.HasValue)
                                {
                                    lineGuidToId[guid.Value] = id;
                                }
                            }
                        }

                        if (request.LineSerials != null && request.LineSerials.Count > 0)
                        {
                            var serials = new List<ShLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("ShHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("ShHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("ShHeaderLineReferenceMissing"), 400);
                                }

                                var serial = _mapper.Map<ShLineSerial>(s);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.ShLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<ShTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                var tline = _mapper.Map<ShTerminalLine>(t);
                                tline.HeaderId = header.Id;
                                tlines.Add(tline);
                            }
                            await _unitOfWork.ShTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        var dto = _mapper.Map<ShHeaderDto>(header);
                        return ApiResponse<ShHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShHeaderGenerateCompletedSuccessfully"));
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
                return ApiResponse<ShHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderGenerateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<int>> BulkCreateShipmentAsync(BulkCreateShRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        // ============================================
                        // 1. VALIDATION
                        // ============================================
                        if (request == null || request.Header == null)
                        {
                            return ApiResponse<int>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidModelState"),
                                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                                400);
                        }

                        // ============================================
                        // 1.1. CHECK ERP APPROVAL REQUIREMENT
                        // ============================================
                        var shParameter = await _unitOfWork.ShParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var shHeader = _mapper.Map<ShHeader>(request.Header);
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        shHeader.IsPendingApproval = shParameter != null && shParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.ShHeaders.AddAsync(shHeader);
                        await _unitOfWork.SaveChangesAsync();

                        if (shHeader?.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<int>.ErrorResult(
                                _localizationService.GetLocalizedString("ShHeaderBulkCreateError"),
                                _localizationService.GetLocalizedString("HeaderInsertFailed"),
                                500);
                        }

                        // ============================================
                        // 3. CREATE LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<ShLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = _mapper.Map<ShLine>(lineDto);
                                line.HeaderId = shHeader.Id;
                                lines.Add(line);
                            }

                            await _unitOfWork.ShLines.AddRangeAsync(lines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ClientKey -> Id and ClientGuid -> Id mappings
                            for (int i = 0; i < request.Lines.Count; i++)
                            {
                                var key = request.Lines[i].ClientKey;
                                var guid = request.Lines[i].ClientGuid;
                                var id = lines[i].Id;

                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    lineKeyToId[key] = id;
                                }
                                if (guid.HasValue)
                                {
                                    lineGuidToId[guid.Value] = id;
                                }
                            }
                        }

                        // ============================================
                        // 4. CREATE LINE SERIALS
                        // ============================================
                        if (request.LineSerials != null && request.LineSerials.Count > 0)
                        {
                            var serials = new List<ShLineSerial>(request.LineSerials.Count);
                            foreach (var serialDto in request.LineSerials)
                            {
                                // LineSerial requires LineId - find by LineGroupGuid first, then LineClientKey
                                long lineId;
                                if (serialDto.LineGroupGuid.HasValue)
                                {
                                    if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderLineGroupGuidNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }
                                else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(serialDto.LineClientKey, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderLineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }
                                else
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ShHeaderLineReferenceMissing"),
                                        400);
                                }

                                var serial = _mapper.Map<ShLineSerial>(serialDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }

                            await _unitOfWork.ShLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 5. CREATE IMPORT LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineGuidToId = new Dictionary<Guid, long>();
                        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var routeGuidToImportLineId = new Dictionary<Guid, long>();

                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<ShImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                // ClientKey is required for correlation
                                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineClientKeyMissing"),
                                        400);
                                }

                                // LineClientKey/LineGroupGuid is optional - if provided, validate and link to Line
                                long? lineId = null;
                                if (importDto.LineGroupGuid.HasValue)
                                {
                                    if (!lineGuidToId.TryGetValue(importDto.LineGroupGuid.Value, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderLineGroupGuidNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }
                                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderLineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }

                                var importLine = _mapper.Map<ShImportLine>(importDto);
                                importLine.HeaderId = shHeader.Id;
                                importLine.LineId = lineId;
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.ShImportLines.AddRangeAsync(importLines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ClientKey -> Id, ClientGroupGuid -> Id, RouteClientKey -> Id, RouteGroupGuid -> Id mappings
                            for (int i = 0; i < request.ImportLines.Count; i++)
                            {
                                var id = importLines[i].Id;
                                var clientKey = request.ImportLines[i].ClientKey;
                                var clientGuid = request.ImportLines[i].ClientGroupGuid;
                                var routeKey = request.ImportLines[i].RouteClientKey;
                                var routeGuid = request.ImportLines[i].RouteGroupGuid;

                                if (!string.IsNullOrWhiteSpace(clientKey))
                                {
                                    importLineKeyToId[clientKey] = id;
                                }
                                if (clientGuid.HasValue)
                                {
                                    importLineGuidToId[clientGuid.Value] = id;
                                }
                                if (!string.IsNullOrWhiteSpace(routeKey))
                                {
                                    routeKeyToImportLineId[routeKey] = id;
                                }
                                if (routeGuid.HasValue)
                                {
                                    routeGuidToImportLineId[routeGuid.Value] = id;
                                }
                            }
                        }

                        // ============================================
                        // 6. CREATE ROUTES
                        // ============================================
                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var routes = new List<ShRoute>(request.Routes.Count);
                            foreach (var routeDto in request.Routes)
                            {
                                // Find ImportLineId by priority: ImportLineGroupGuid > ImportLineClientKey > RouteGroupGuid > RouteClientKey
                                long? importLineId = null;

                                if (routeDto.ImportLineGroupGuid.HasValue)
                                {
                                    if (!importLineGuidToId.TryGetValue(routeDto.ImportLineGroupGuid.Value, out var foundImportLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderRouteGroupGuidNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }
                                else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                                {
                                    if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey, out var foundImportLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderRouteGroupGuidNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }
                                else if (routeDto.ClientGroupGuid.HasValue)
                                {
                                    if (!routeGuidToImportLineId.TryGetValue(routeDto.ClientGroupGuid.Value, out var foundImportLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderRouteGroupGuidNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }
                                else if (!string.IsNullOrWhiteSpace(routeDto.ClientKey))
                                {
                                    if (!routeKeyToImportLineId.TryGetValue(routeDto.ClientKey, out var foundImportLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("ShHeaderRouteGroupGuidNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }

                                if (!importLineId.HasValue)
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("ShHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ShHeaderLineReferenceMissing"),
                                        400);
                                }

                                var route = _mapper.Map<ShRoute>(routeDto);
                                route.ImportLineId = importLineId.Value;
                                routes.Add(route);
                            }

                            await _unitOfWork.ShRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 7. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        return ApiResponse<int>.SuccessResult(
                            1,
                            _localizationService.GetLocalizedString("ShHeaderBulkCreateCompletedSuccessfully"));
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
                var inner = ex.InnerException?.Message ?? string.Empty;
                var combined = string.IsNullOrWhiteSpace(inner) ? ex.Message : $"{ex.Message} | Inner: {inner}";
                return ApiResponse<int>.ErrorResult(
                    _localizationService.GetLocalizedString("ShHeaderBulkCreateError"),
                    combined,
                    500);
            }
        }
    }
}
