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
    public class PrHeaderService : IPrHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public PrHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.PrHeaders.FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<PrHeaderDto>>(entities);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                return ApiResponse<IEnumerable<PrHeaderDto>>.SuccessResult(enrichedWarehouse.Data ?? dtos, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.PrHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<PrHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<PrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<PrHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<PrHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<PrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
                    return ApiResponse<PrHeaderDto>.ErrorResult(notFound, notFound, 404);
                }
                var dto = _mapper.Map<PrHeaderDto>(entity);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PrHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                var finalDto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { finalDto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PrHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                finalDto = enrichedWarehouse.Data?.FirstOrDefault() ?? finalDto;

                return ApiResponse<PrHeaderDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> CreateAsync(CreatePrHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrHeaderDto>(entity);
                return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> UpdateAsync(long id, UpdatePrHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
                    return ApiResponse<PrHeaderDto>.ErrorResult(notFound, notFound, 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrHeaderDto>(entity);
                return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PrHeaders.ExistsAsync(id);
                if (!exists)
                {
                    var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }
                var importLines = await _unitOfWork.PrImportLines.FindAsync(x => x.HeaderId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("PrHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.PrHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                // ============================================
                // CHECK ERP APPROVAL REQUIREMENT
                // ============================================
                var prParameter = await _unitOfWork.PrParameters
                    .AsQueryable()
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync();

                // ============================================
                // VALIDATE LINE SERIAL VS ROUTE QUANTITIES
                // ============================================
                // Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
                // Normalize null values to false
                bool skipValidation = (prParameter?.AllowLessQuantityBasedOnOrder ?? false) 
                    && (prParameter?.AllowMoreQuantityBasedOnOrder ?? false);

                // Normalize RequireAllOrderItemsCollected
                bool requireAllOrderItemsCollected = prParameter?.RequireAllOrderItemsCollected ?? false;

                if (!skipValidation)
                {
                    var lines = await _unitOfWork.PrLines
                        .AsQueryable()
                        .Where(l => l.HeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var line in lines)
                    {
                        // Get total quantity of LineSerials for this Line
                        var totalLineSerialQuantity = await _unitOfWork.PrLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId == line.Id)
                            .SumAsync(ls => ls.Quantity);

                        // Get total quantity of Routes for ImportLines linked to this Line
                        var totalRouteQuantity = await _unitOfWork.PrRoutes
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
                                var msg = _localizationService.GetLocalizedString("PrHeaderAllOrderItemsMustBeCollected", 
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
                        bool allowLess = prParameter?.AllowLessQuantityBasedOnOrder ?? false;
                        bool allowMore = prParameter?.AllowMoreQuantityBasedOnOrder ?? false;
                        
                        bool quantityMismatch = false;
                        string localizedMessage = string.Empty;
                        string exceptionMessage = string.Empty;

                        if (!allowLess && !allowMore)
                        {
                            // Both false: Exact match required (==)
                            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("PrHeaderQuantityExactMatchRequired", 
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
                                localizedMessage = _localizationService.GetLocalizedString("PrHeaderQuantityCannotBeGreater", 
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
                                localizedMessage = _localizationService.GetLocalizedString("PrHeaderQuantityCannotBeLess", 
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
                entity.IsPendingApproval = prParameter != null && prParameter.RequireApprovalBeforeErp;

                _unitOfWork.PrHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> GenerateProductionOrderAsync(GenerateProductionOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                        var header = _mapper.Map<PrHeader>(request.Header);
                        header.CreatedDate = DateTime.UtcNow;
                        header.IsDeleted = false;
                        await _unitOfWork.PrHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<PrLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                            var line = _mapper.Map<PrLine>(l);
                            line.HeaderId = header.Id;
                                lines.Add(line);
                            }
                            await _unitOfWork.PrLines.AddRangeAsync(lines);
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
                            var serials = new List<PrLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    if (!lineGuidToId.TryGetValue(s.LineGroupGuid.Value, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("PrHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("PrHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("PrHeaderLineReferenceMissing"), 400);
                                }

                            var serial = _mapper.Map<PrLineSerial>(s);
                            serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.PrLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.HeaderSerials != null && request.HeaderSerials.Count > 0)
                        {
                            var headerSerials = new List<PrHeaderSerial>(request.HeaderSerials.Count);
                            foreach (var hs in request.HeaderSerials)
                            {
                            var hSerial = _mapper.Map<PrHeaderSerial>(hs);
                            hSerial.HeaderId = header.Id;
                                headerSerials.Add(hSerial);
                            }
                            await _unitOfWork.PrHeaderSerials.AddRangeAsync(headerSerials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<PrTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                            var tline = _mapper.Map<PrTerminalLine>(t);
                            tline.HeaderId = header.Id;
                            tlines.Add(tline);
                            }
                            await _unitOfWork.PrTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        var dto = _mapper.Map<PrHeaderDto>(header);
                        return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderGenerateCompletedSuccessfully"));
                    }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderGenerateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> BulkPrGenerateAsync(BulkPrGenerateRequestDto request)
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
                            return ApiResponse<PrHeaderDto>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidModelState"),
                                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                                400);
                        }

                        // ============================================
                        // 1.1. CHECK ERP APPROVAL REQUIREMENT
                        // ============================================
                        var prParameter = await _unitOfWork.PrParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var headerEntity = _mapper.Map<PrHeader>(request.Header.Header);
                        headerEntity.CreatedDate = DateTime.UtcNow;
                        headerEntity.IsDeleted = false;
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        headerEntity.IsPendingApproval = prParameter != null && prParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.PrHeaders.AddAsync(headerEntity);
                        await _unitOfWork.SaveChangesAsync();

                        if (headerEntity?.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<PrHeaderDto>.ErrorResult(
                                _localizationService.GetLocalizedString("PrHeaderGenerateError"),
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
                            var lines = new List<PrLine>(request.Lines.Count);
                            var lineKeys = new List<Guid>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                if (!headerKeyToId.TryGetValue(lineDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("HeaderKeyNotFound"),
                                        400);
                                }
                                var line = _mapper.Map<PrLine>(lineDto);
                                line.HeaderId = hdrId;
                                lines.Add(line);
                                lineKeys.Add(lineDto.LineKey);
                            }

                            await _unitOfWork.PrLines.AddRangeAsync(lines);
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
                            var serials = new List<PrLineSerial>(request.LineSerials.Count);
                            foreach (var serialDto in request.LineSerials)
                            {
                                if (!lineKeyToId.TryGetValue(serialDto.LineKey, out var lineId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("PrHeaderLineClientKeyNotFound"),
                                        400);
                                }

                                var serial = _mapper.Map<PrLineSerial>(serialDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }

                            await _unitOfWork.PrLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 5. CREATE HEADER SERIALS
                        // ============================================
                        if (request.HeaderSerials != null && request.HeaderSerials.Count > 0)
                        {
                            var headerSerials = new List<PrHeaderSerial>(request.HeaderSerials.Count);
                            foreach (var headerSerialDto in request.HeaderSerials)
                            {
                                if (!headerKeyToId.TryGetValue(headerSerialDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("HeaderKeyNotFound"),
                                        400);
                                }

                                var hSerial = _mapper.Map<PrHeaderSerial>(headerSerialDto);
                                hSerial.HeaderId = hdrId;
                                headerSerials.Add(hSerial);
                            }

                            await _unitOfWork.PrHeaderSerials.AddRangeAsync(headerSerials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 6. CREATE TERMINAL LINES
                        // ============================================
                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<PrTerminalLine>(request.TerminalLines.Count);
                            foreach (var terminalLineDto in request.TerminalLines)
                            {
                                if (!headerKeyToId.TryGetValue(terminalLineDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("HeaderKeyNotFound"),
                                        400);
                                }

                                var tline = _mapper.Map<PrTerminalLine>(terminalLineDto);
                                tline.HeaderId = hdrId;
                                tlines.Add(tline);
                            }

                            await _unitOfWork.PrTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 7. CREATE IMPORT LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var importLineKeyToId = new Dictionary<Guid, long>();
                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<PrImportLine>(request.ImportLines.Count);
                            var importKeys = new List<Guid>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                if (!headerKeyToId.TryGetValue(importDto.HeaderKey, out var hdrId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
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
                                        return ApiResponse<PrHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("PrHeaderLineClientKeyNotFound"),
                                            400);
                                    }
                                    linkedLineId = foundLineId;
                                }

                                var importLine = _mapper.Map<PrImportLine>(importDto);
                                importLine.HeaderId = hdrId;
                                importLine.LineId = linkedLineId;
                                importLines.Add(importLine);
                                importKeys.Add(importDto.ImportLineKey);
                            }

                            await _unitOfWork.PrImportLines.AddRangeAsync(importLines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ImportLineKey -> Id mapping
                            for (int i = 0; i < importLines.Count; i++)
                            {
                                importLineKeyToId[importKeys[i]] = importLines[i].Id;
                            }
                        }

                        // ============================================
                        // 8. CREATE ROUTES
                        // ============================================
                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var routes = new List<PrRoute>(request.Routes.Count);
                            foreach (var routeDto in request.Routes)
                            {
                                if (!importLineKeyToId.TryGetValue(routeDto.ImportLineKey, out var importLineId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<PrHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("PrHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineKeyNotFound"),
                                        400);
                                }

                                var route = _mapper.Map<PrRoute>(routeDto);
                                route.ImportLineId = importLineId;
                                routes.Add(route);
                            }

                            await _unitOfWork.PrRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 9. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        var dto = _mapper.Map<PrHeaderDto>(headerEntity);
                        return ApiResponse<PrHeaderDto>.SuccessResult(
                            dto,
                            _localizationService.GetLocalizedString("PrHeaderGenerateCompletedSuccessfully"));
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
                return ApiResponse<PrHeaderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PrHeaderGenerateError"),
                    combined,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAssignedProductionOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.PrHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.PrTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<PrHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<PrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderAssignedOrdersRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrAssignedProductionOrderLinesDto>> GetAssignedProductionOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.PrLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var lineDtos = _mapper.Map<IEnumerable<PrLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<PrLineSerial> lineSerials = Array.Empty<PrLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.PrLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.PrImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var importLineDtos = _mapper.Map<IEnumerable<PrImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<PrRoute> routes = Array.Empty<PrRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.PrRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var dto = new PrAssignedProductionOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<PrLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<PrRouteDto>>(routes)
                };

                return ApiResponse<PrAssignedProductionOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrAssignedProductionOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.PrHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<PrHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<PrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<PrHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<PrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PrHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                var entity = await _unitOfWork.PrHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PrHeaderNotFound");
                    return ApiResponse<PrHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("PrHeaderApprovalUpdateError");
                    return ApiResponse<PrHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.PrHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PrHeaderDto>(entity);
                return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderApprovalUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }
   
    }
}
