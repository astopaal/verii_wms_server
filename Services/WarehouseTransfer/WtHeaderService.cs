using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Collections.Generic;

namespace WMS_WEBAPI.Services
{
    public class WtHeaderService : IWtHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        private readonly INotificationService _notificationService;

        public WtHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 0) request.PageNumber = 0;
                if (request.PageSize < 1) request.PageSize = 20;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.WtHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<WtHeaderDto>>(items);

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<WtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;

                return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtHeaders.GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
                    return ApiResponse<WtHeaderDto>.ErrorResult(notFound, notFound, 404);
                }

                var dto = _mapper.Map<WtHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                }
                if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.YearCode?.Length != 4)
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.PlannedDate == default)
                {
                    return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                var entity = _mapper.Map<WtHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;

                await _unitOfWork.WtHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtHeaderDto>(entity);
                return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
                    return ApiResponse<WtHeaderDto>.ErrorResult(notFound, notFound, 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.WtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtHeaderDto>(entity);
                return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WtHeaders.ExistsAsync(id);
                if (!exists)
                {
                    var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                var importLines = await _unitOfWork.WtImportLines.FindAsync(x => x.HeaderId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WtHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                await _unitOfWork.WtHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                // ============================================
                // CHECK ERP APPROVAL REQUIREMENT
                // ============================================
                var wtParameter = await _unitOfWork.WtParameters
                    .AsQueryable()
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync();

                // ============================================
                // VALIDATE LINE SERIAL VS ROUTE QUANTITIES
                // ============================================
                // Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
                // Normalize null values to false
                bool skipValidation = (wtParameter?.AllowLessQuantityBasedOnOrder ?? false) 
                    && (wtParameter?.AllowMoreQuantityBasedOnOrder ?? false);

                // Normalize RequireAllOrderItemsCollected
                bool requireAllOrderItemsCollected = wtParameter?.RequireAllOrderItemsCollected ?? false;

                if (!skipValidation)
                {
                    var lines = await _unitOfWork.WtLines
                        .AsQueryable()
                        .Where(l => l.HeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var line in lines)
                    {
                        // Get total quantity of LineSerials for this Line
                        var totalLineSerialQuantity = await _unitOfWork.WtLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId == line.Id)
                            .SumAsync(ls => ls.Quantity);

                        // Get total quantity of Routes for ImportLines linked to this Line
                        var totalRouteQuantity = await _unitOfWork.WtRoutes
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
                                var msg = _localizationService.GetLocalizedString("WtHeaderAllOrderItemsMustBeCollected", 
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
                        bool allowLess = wtParameter?.AllowLessQuantityBasedOnOrder ?? false;
                        bool allowMore = wtParameter?.AllowMoreQuantityBasedOnOrder ?? false;
                        
                        bool quantityMismatch = false;
                        string localizedMessage = string.Empty;
                        string exceptionMessage = string.Empty;

                        if (!allowLess && !allowMore)
                        {
                            // Both false: Exact match required (==)
                            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("WtHeaderQuantityExactMatchRequired", 
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
                            // Route <= LineSerial (Route can be less or equal to LineSerial) can
                            // Error if Route > LineSerial
                            if (totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("WtHeaderQuantityCannotBeGreater", 
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
                                localizedMessage = _localizationService.GetLocalizedString("WtHeaderQuantityCannotBeLess", 
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
                    entity.IsPendingApproval = wtParameter!= null && wtParameter.RequireApprovalBeforeErp;
                    _unitOfWork.WtHeaders.Update(entity);

                    // Update package status to Shipped
                    var package = _unitOfWork.PHeaders.AsQueryable()
                        .Where(p => p.SourceHeaderId == entity.Id && !p.IsDeleted && p.SourceType == PHeaderSourceType.WT)
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
                            Title = _localizationService.GetLocalizedString("WtDoneNotificationTitle", orderNumber),
                            Message = _localizationService.GetLocalizedString("WtDoneNotificationMessage", orderNumber),
                            TitleKey = "WtDoneNotificationTitle",
                            MessageKey = "WtDoneNotificationMessage",
                            Channel = NotificationChannel.Web,
                            Severity = NotificationSeverity.Info,
                            RecipientUserId = entity.CreatedBy.Value,
                            RelatedEntityType = NotificationEntityType.WTDone,
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

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtHeaderCompletedSuccessfully"));
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedTransferOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.WtHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.WtTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderAssignedOrdersRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtAssignedTransferOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.WtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var lineDtos = _mapper.Map<IEnumerable<WtLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<WtLineSerial> lineSerials = Array.Empty<WtLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.WtLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var importLineDtos = _mapper.Map<IEnumerable<WtImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<WtRoute> routes = Array.Empty<WtRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.WtRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var dto = new WtAssignedTransferOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<WtLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<WtRouteDto>>(routes)
                };

                return ApiResponse<WtAssignedTransferOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtAssignedTransferOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        } 

        public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.WtHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted 
                    && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WtHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                // Tracking ile yükle (navigation property'ler yüklenmeyecek)
                var entity = await _unitOfWork.WtHeaders
                    .AsQueryable()
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
                    
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("WtHeaderNotFound");
                    return ApiResponse<WtHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("WtHeaderApprovalUpdateError");
                    return ApiResponse<WtHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.WtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtHeaderDto>(entity);
                return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderApprovalUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<WtHeader>(request.Header);
                        await _unitOfWork.WtHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WtLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = _mapper.Map<WtLine>(l);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }
                            await _unitOfWork.WtLines.AddRangeAsync(lines);
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
                            var serials = new List<WtLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineReferenceMissing"), 400);
                                }

                                var serial = _mapper.Map<WtLineSerial>(s);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.WtLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        List<Notification> createdNotifications = new List<Notification>();
                        
                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<WtTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                var tline = _mapper.Map<WtTerminalLine>(t);
                                tline.HeaderId = header.Id;
                                tlines.Add(tline);
                            }
                            await _unitOfWork.WtTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();

                            // Create and add notifications for each terminal line
                            var orderNumber = header.Id.ToString();
                            createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                                tlines,
                                orderNumber,
                                NotificationEntityType.WTHeader,
                                "WT_HEADER",
                                "WtHeaderNotificationTitle",
                                "WtHeaderNotificationMessage"
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

                        var dto = _mapper.Map<WtHeaderDto>(header);
                        return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderGenerateCompletedSuccessfully"));
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
                return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderGenerateError"), ex.Message ?? string.Empty, 500);
            }
        }
    
        public async Task<ApiResponse<WtHeaderDto>> BulkWtGenerateAsync(BulkWtGenerateRequestDto request)
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
                        if (request == null || request.Header == null || request.Header.Header == null)
                        {
                            return ApiResponse<WtHeaderDto>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidModelState"),
                                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                                400);
                        }

                        // ============================================
                        // 1.1. CHECK ERP APPROVAL REQUIREMENT
                        // ============================================
                        var wtParameter = await _unitOfWork.WtParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var headerEntity = _mapper.Map<WtHeader>(request.Header.Header);
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        headerEntity.IsPendingApproval = wtParameter != null && wtParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.WtHeaders.AddAsync(headerEntity);
                        await _unitOfWork.SaveChangesAsync();

                        if (headerEntity == null || headerEntity.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<WtHeaderDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtHeaderGenerateError"),
                                _localizationService.GetLocalizedString("HeaderInsertFailed"),
                                500);
                        }

                        if (request?.Header?.HeaderKey == null)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<WtHeaderDto>.ErrorResult(
                                _localizationService.GetLocalizedString("WtHeaderGenerateError"),
                                _localizationService.GetLocalizedString("HeaderInsertFailed"),
                                500);
                        }

                        var headerKeyToId = new Dictionary<Guid, long> { { request.Header.HeaderKey, headerEntity.Id } };

                        // ============================================
                        // 3. CREATE LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var lineKeyToId = new Dictionary<Guid, long>();
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WtLine>(request.Lines.Count);
                            var lineKeys = new List<Guid>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                if (!headerKeyToId.TryGetValue(lineDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<WtHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("HeaderKeyNotFound"),
                                        400);
                                }
                                var line = _mapper.Map<WtLine>(lineDto);
                                line.HeaderId = hdrId;
                                lines.Add(line);
                                lineKeys.Add(lineDto.LineKey);
                            }

                            await _unitOfWork.WtLines.AddRangeAsync(lines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build LineKey -> Id mapping
                            for (int i = 0; i < lines.Count; i++)
                            {
                                lineKeyToId[lineKeys[i]] = lines[i].Id;
                            }
                        }

                        // ============================================
                        // 4. CREATE LINE SERIALS
                        // ============================================
                        if (request.LineSerials != null && request.LineSerials.Count > 0)
                        {
                            var serials = new List<WtLineSerial>(request.LineSerials.Count);
                            foreach (var serialDto in request.LineSerials)
                            {
                                if (!lineKeyToId.TryGetValue(serialDto.LineKey, out var lineId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<WtHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("LineKeyNotFound"),
                                        400);
                                }

                                var serial = _mapper.Map<WtLineSerial>(serialDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }

                            await _unitOfWork.WtLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 5. CREATE IMPORT LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var importLineKeyToId = new Dictionary<Guid, long>();
                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<WtImportLine>(request.ImportLines.Count);
                            var importKeys = new List<Guid>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                if (!headerKeyToId.TryGetValue(importDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<WtHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("HeaderKeyNotFound"),
                                        400);
                                }

                                // LineKey is optional - if provided, validate and link to Line
                                long? linkedLineId = null;
                                if (importDto.LineKey.HasValue)
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineKey.Value, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<WtHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("LineKeyNotFound"),
                                            400);
                                    }
                                    linkedLineId = foundLineId;
                                }

                                var importLine = _mapper.Map<WtImportLine>(importDto);
                                importLine.HeaderId = hdrId;
                                importLine.LineId = linkedLineId;
                                importLines.Add(importLine);
                                importKeys.Add(importDto.ImportLineKey);
                            }

                            await _unitOfWork.WtImportLines.AddRangeAsync(importLines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ImportLineKey -> Id mapping
                            for (int i = 0; i < importLines.Count; i++)
                            {
                                importLineKeyToId[importKeys[i]] = importLines[i].Id;
                            }
                        }

                        // ============================================
                        // 6. CREATE ROUTES
                        // ============================================
                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var routes = new List<WtRoute>(request.Routes.Count);
                            foreach (var routeDto in request.Routes)
                            {
                                if (!importLineKeyToId.TryGetValue(routeDto.ImportLineKey, out var importLineId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<WtHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineKeyNotFound"),
                                        400);
                                }

                                var route = _mapper.Map<WtRoute>(routeDto);
                                route.ImportLineId = importLineId;
                                if (string.IsNullOrEmpty(route.ScannedBarcode))
                                {
                                    route.ScannedBarcode = string.Empty;
                                }
                                routes.Add(route);
                            }

                            await _unitOfWork.WtRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }



                        // ============================================
                        // 7. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        var dto = _mapper.Map<WtHeaderDto>(headerEntity);
                        return ApiResponse<WtHeaderDto>.SuccessResult(
                            dto,
                            _localizationService.GetLocalizedString("WtHeaderGenerateCompletedSuccessfully"));
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
                return ApiResponse<WtHeaderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("WtHeaderGenerateError"),
                    combined,
                    500);
            }
        }

    }
}
