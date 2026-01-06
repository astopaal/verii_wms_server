using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Security.Claims;

namespace WMS_WEBAPI.Services
{
    public class WiHeaderService : IWiHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        private readonly INotificationService _notificationService;

        public WiHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.WiHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WiHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WiHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WiHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiHeaders.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderNotFound"), _localizationService.GetLocalizedString("WiHeaderNotFound"), 404);
                var dto = _mapper.Map<WiHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<WiHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<WiHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByInboundTypeAsync(string inboundType)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.InboundType == inboundType && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByAccountCodeAsync(string accountCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.AccountCode == accountCode && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiHeaderDto>> CreateAsync(CreateWiHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiHeader>(createDto);
                var created = await _unitOfWork.WiHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiHeaderDto>(created);
                return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiHeaderDto>> UpdateAsync(long id, UpdateWiHeaderDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiHeaders.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderNotFound"), _localizationService.GetLocalizedString("WiHeaderNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiHeaderDto>(entity);
                return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var importLines = await _unitOfWork.WiImportLines.FindAsync(x => x.HeaderId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WiHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                await _unitOfWork.WiHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("WiHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                // ============================================
                // CHECK ERP APPROVAL REQUIREMENT
                // ============================================
                var wiParameter = await _unitOfWork.WiParameters
                    .AsQueryable()
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync();

                // ============================================
                // VALIDATE LINE SERIAL VS ROUTE QUANTITIES
                // ============================================
                // Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
                // Normalize null values to false
                bool skipValidation = (wiParameter?.AllowLessQuantityBasedOnOrder ?? false) 
                    && (wiParameter?.AllowMoreQuantityBasedOnOrder ?? false);

                // Normalize RequireAllOrderItemsCollected
                bool requireAllOrderItemsCollected = wiParameter?.RequireAllOrderItemsCollected ?? false;

                if (!skipValidation)
                {
                    var lines = await _unitOfWork.WiLines
                        .AsQueryable()
                        .Where(l => l.HeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var line in lines)
                    {
                        // Get total quantity of LineSerials for this Line
                        var totalLineSerialQuantity = await _unitOfWork.WiLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId == line.Id)
                            .SumAsync(ls => ls.Quantity);

                        // Get total quantity of Routes for ImportLines linked to this Line
                        var totalRouteQuantity = await _unitOfWork.WiRoutes
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
                                var msg = _localizationService.GetLocalizedString("WiHeaderAllOrderItemsMustBeCollected", 
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
                        bool allowLess = wiParameter?.AllowLessQuantityBasedOnOrder ?? false;
                        bool allowMore = wiParameter?.AllowMoreQuantityBasedOnOrder ?? false;
                        
                        bool quantityMismatch = false;
                        string localizedMessage = string.Empty;
                        string exceptionMessage = string.Empty;

                        if (!allowLess && !allowMore)
                        {
                            // Both false: Exact match required (==)
                            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("WiHeaderQuantityExactMatchRequired", 
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
                                localizedMessage = _localizationService.GetLocalizedString("WiHeaderQuantityCannotBeGreater", 
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
                                localizedMessage = _localizationService.GetLocalizedString("WiHeaderQuantityCannotBeLess", 
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

                // ============================================
                // TRANSACTION: Start transaction for write operations
                // ============================================
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    entity.IsCompleted = true;
                    entity.CompletionDate = DateTime.UtcNow;
                    
                    // Set IsPendingApproval based on parameter requirement
                    entity.IsPendingApproval = wiParameter != null && wiParameter.RequireApprovalBeforeErp;
                    _unitOfWork.WiHeaders.Update(entity);

                    // Update package status to Shipped
                    var package = _unitOfWork.PHeaders.AsQueryable()
                        .Where(p => p.SourceHeaderId == entity.Id && !p.IsDeleted && p.SourceType == PHeaderSourceType.WI)
                        .FirstOrDefault();
                    if (package != null)
                    {
                        package.Status = PHeaderStatus.Shipped;
                        _unitOfWork.PHeaders.Update(package);
                    }

                    // Create notification for the user who created the order
                    Notification? notification = null;
                    if (entity.CreatedBy.HasValue)
                    {
                        var orderNumber = entity.Id.ToString();
                        notification = new Notification
                        {
                            Title = _localizationService.GetLocalizedString("WiDoneNotificationTitle", orderNumber),
                            Message = _localizationService.GetLocalizedString("WiDoneNotificationMessage", orderNumber),
                            TitleKey = "WiDoneNotificationTitle",
                            MessageKey = "WiDoneNotificationMessage",
                            Channel = NotificationChannel.Web,
                            Severity = NotificationSeverity.Info,
                            RecipientUserId = entity.CreatedBy.Value,
                            RelatedEntityType = NotificationEntityType.WIDone,
                            RelatedEntityId = entity.Id,
                            DeliveredAt = DateTime.UtcNow
                        };

                        await _unitOfWork.Notifications.AddAsync(notification);
                    }

                    // Single SaveChanges for both header update and notification
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    // Publish SignalR notification after transaction is committed
                    if (notification != null)
                    {
                        await _notificationService.PublishSignalRNotificationsAsync(new[] { notification });
                    }

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiHeaderCompletedSuccessfully"));
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.WiHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.WiTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderAssignedOrdersRetrievalError"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.WiLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<WiLineSerial> lineSerials = Array.Empty<WiLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.WiLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.WiImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<WiRoute> routes = Array.Empty<WiRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.WiRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var lineDtos = _mapper.Map<IEnumerable<WiLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var importLineDtos = _mapper.Map<IEnumerable<WiImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var dto = new WiAssignedOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<WiLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<WiRouteDto>>(routes)
                };

                return ApiResponse<WiAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.WiHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WiHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WiHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WiHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WiHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                // Tracking ile yükle (navigation property'ler yüklenmeyecek)
                var entity = await _unitOfWork.WiHeaders
                    .AsQueryable()
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
                    
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("WiHeaderNotFound");
                    return ApiResponse<WiHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("WiHeaderApprovalUpdateError");
                    return ApiResponse<WiHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.WiHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WiHeaderDto>(entity);
                return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderApprovalUpdateError"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiHeaderDto>> GenerateWarehouseInboundOrderAsync(GenerateWarehouseInboundOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<WiHeader>(request.Header);
                        await _unitOfWork.WiHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WiLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = _mapper.Map<WiLine>(l);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }
                            await _unitOfWork.WiLines.AddRangeAsync(lines);
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
                            var serials = new List<WiLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WiHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WiHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<WiHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                        400
                                    );
                                }

                                var serial = _mapper.Map<WiLineSerial>(s);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.WiLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        List<Notification> createdNotifications = new List<Notification>();
                        
                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<WiTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                var tline = _mapper.Map<WiTerminalLine>(t);
                                tline.HeaderId = header.Id;
                                tlines.Add(tline);
                            }
                            await _unitOfWork.WiTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();

                            // Create and add notifications for each terminal line
                            var orderNumber = header.Id.ToString();
                            createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                                tlines,
                                orderNumber,
                                NotificationEntityType.WIHeader,
                                "WI_HEADER",
                                "WiHeaderNotificationTitle",
                                "WiHeaderNotificationMessage"
                            );
                            
                            // Save notifications to database (they will be committed with transaction)
                            if (createdNotifications.Count > 0)
                            {
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        // Publish SignalR notifications after transaction is committed
                        if (createdNotifications.Count > 0)
                        {
                            await _notificationService.PublishSignalRNotificationsForCreatedNotificationsAsync(createdNotifications);
                        }

                        var dto = _mapper.Map<WiHeaderDto>(header);
                        return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderCreatedSuccessfully"));
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
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<int>> BulkCreateWarehouseInboundAsync(BulkCreateWiRequestDto request)
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
                        var wiParameter = await _unitOfWork.WiParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var header = _mapper.Map<WiHeader>(request.Header);
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        header.IsPendingApproval = wiParameter != null && wiParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.WiHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        if (header?.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<int>.ErrorResult(
                                _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
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
                            var lines = new List<WiLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = _mapper.Map<WiLine>(lineDto);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }

                            await _unitOfWork.WiLines.AddRangeAsync(lines);
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
                            var serials = new List<WiLineSerial>(request.LineSerials.Count);
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("LineGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("LineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }
                                else
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("LineReferenceMissing"),
                                        400);
                                }

                                var serial = _mapper.Map<WiLineSerial>(serialDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }

                            await _unitOfWork.WiLineSerials.AddRangeAsync(serials);
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
                            var importLines = new List<WiImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                // ClientKey is required for correlation
                                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("LineGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("LineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }

                                var importLine = _mapper.Map<WiImportLine>(importDto);
                                importLine.HeaderId = header.Id;
                                importLine.LineId = lineId;
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.WiImportLines.AddRangeAsync(importLines);
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
                            var routes = new List<WiRoute>(request.Routes.Count);
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("ImportLineGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("ImportLineClientKeyNotFound"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("RouteGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("RouteClientKeyNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }

                                if (!importLineId.HasValue)
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("ImportLineReferenceMissing"),
                                        400);
                                }

                                var route = _mapper.Map<WiRoute>(routeDto);
                                route.ImportLineId = importLineId.Value;
                                routes.Add(route);
                            }

                            await _unitOfWork.WiRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 7. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        return ApiResponse<int>.SuccessResult(
                            1,
                            _localizationService.GetLocalizedString("WiHeaderCreatedSuccessfully"));
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
                    _localizationService.GetLocalizedString("WiHeaderErrorOccurred"),
                    combined,
                    500);
            }
        }
  
    }
}
