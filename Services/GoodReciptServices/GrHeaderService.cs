using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
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

        public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.GrHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<GrHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<GrHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result,_localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderRetrievalError"), ex.Message, 500);
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
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderRetrievalError"), ex.Message, 500);
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
                
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { grHeaderDto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<GrHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                grHeaderDto = enrichedCustomer.Data?.FirstOrDefault() ?? grHeaderDto;
                
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
                
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { grHeaderDto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<GrHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                grHeaderDto = enrichedCustomer.Data?.FirstOrDefault() ?? grHeaderDto;
                
                return ApiResponse<GrHeaderDto>.SuccessResult(grHeaderDto,_localizationService.GetLocalizedString("GrHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderUpdateError"), ex.Message, 500);
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
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderSoftDeletionError"), ex.Message, 500);
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
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderRetrievalError"), ex.Message, 500);
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
                        // ============================================
                        // 1. VALIDATION
                        // ============================================
                        if (request == null || request.Header == null)
                        {
                            return ApiResponse<long>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidModelState"),
                                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                                400);
                        }

                        // Set default BranchCode if empty
                        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
                        {
                            request.Header.BranchCode = "0";
                        }

                        if (string.IsNullOrWhiteSpace(request.Header.CustomerCode))
                        {
                            return ApiResponse<long>.ErrorResult(
                                _localizationService.GetLocalizedString("InvalidModelState"),
                                _localizationService.GetLocalizedString("HeaderFieldsMissing"),
                                400);
                        }

                        // ============================================
                        // 1.1. CHECK ERP APPROVAL REQUIREMENT
                        // ============================================
                        var grParameter = await _unitOfWork.GrParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var header = _mapper.Map<GrHeader>(request.Header);
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        header.IsPendingApproval = grParameter != null && grParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.GrHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        if (header?.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<long>.ErrorResult(
                                _localizationService.GetLocalizedString("GrHeaderCreateError"),
                                _localizationService.GetLocalizedString("HeaderInsertFailed"),
                                500);
                        }

                        

                        // ============================================
                        // 3. CREATE DOCUMENTS
                        // ============================================
                        if (request.Documents?.Count > 0)
                        {
                            var documents = new List<GrImportDocument>(request.Documents.Count);
                            foreach (var doc in request.Documents)
                            {
                                if (doc?.Base64 == null)
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidModelState"),
                                        _localizationService.GetLocalizedString("InvalidModelState"),
                                        400);
                                }
                                documents.Add(new GrImportDocument
                                {
                                    HeaderId = header.Id,
                                    Base64 = doc.Base64
                                });
                            }
                            await _unitOfWork.GrImportDocuments.AddRangeAsync(documents);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 4. CREATE LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<GrLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                if (string.IsNullOrWhiteSpace(lineDto.ClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("LineClientKeyMissing"),
                                        400);
                                }

                                var line = _mapper.Map<GrLine>(lineDto);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }

                            await _unitOfWork.GrLines.AddRangeAsync(lines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ClientKey -> Id mapping
                            for (int i = 0; i < request.Lines.Count; i++)
                            {
                                var key = request.Lines[i].ClientKey;
                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    lineKeyToId[key] = lines[i].Id;
                                }
                            }
                        }

                        // ============================================
                        // 5. CREATE LINE SERIALS (ImportLineId will be updated later)
                        // ============================================
                        var insertedSerials = new List<GrLineSerial>();
                        if (request.SerialLines != null && request.SerialLines.Count > 0)
                        {
                            var serials = new List<GrLineSerial>(request.SerialLines.Count);
                            foreach (var serialDto in request.SerialLines)
                            {
                                if (string.IsNullOrWhiteSpace(serialDto.ImportLineClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineClientKeyMissing"),
                                        400);
                                }

                                var serial = _mapper.Map<GrLineSerial>(serialDto);
                                serials.Add(serial);
                            }

                            await _unitOfWork.GrLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                            insertedSerials = serials;
                        }

                        // ============================================
                        // 6. CREATE IMPORT LINES & BUILD KEY-TO-ID MAPPING
                        // ============================================
                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<GrImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineClientKeyMissing"),
                                        400);
                                }

                                // LineClientKey is optional - if provided, validate and link to Line
                                long? lineId = null;
                                if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey, out var foundLineId))
                                    {
                                        await tx.RollbackAsync();
                                        return ApiResponse<long>.ErrorResult(
                                            _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("LineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }

                                var importLine = _mapper.Map<GrImportLine>(importDto);
                                importLine.HeaderId = header.Id;
                                importLine.LineId = lineId;
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.GrImportLines.AddRangeAsync(importLines);
                            await _unitOfWork.SaveChangesAsync();

                            // Build ClientKey -> Id mapping for ImportLines
                            for (int i = 0; i < request.ImportLines.Count; i++)
                            {
                                var key = request.ImportLines[i].ClientKey;
                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    importLineKeyToId[key] = importLines[i].Id;
                                }
                            }
                        }


                        // ============================================
                        // 8. CREATE ROUTES
                        // ============================================
                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var routes = new List<GrRoute>(request.Routes.Count);
                            foreach (var routeDto in request.Routes)
                            {
                                if (string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineClientKeyMissing"),
                                        400);
                                }

                                if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey, out var importLineId))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<long>.ErrorResult(
                                        _localizationService.GetLocalizedString("InvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("ImportLineClientKeyNotFound"),
                                        400);
                                }

                                var route = _mapper.Map<GrRoute>(routeDto);
                                route.ImportLineId = importLineId;
                                routes.Add(route);
                            }

                            await _unitOfWork.GrRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 9. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        return ApiResponse<long>.SuccessResult(
                            header.Id,
                            _localizationService.GetLocalizedString("GrHeaderCreatedSuccessfully"));
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
                return ApiResponse<long>.ErrorResult(
                    _localizationService.GetLocalizedString("GrHeaderCreationError"),
                    combined,
                    500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.GrHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                // ============================================
                // CHECK ERP APPROVAL REQUIREMENT
                // ============================================
                var grParameter = await _unitOfWork.GrParameters
                    .AsQueryable()
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync();

                // ============================================
                // VALIDATE LINE SERIAL VS ROUTE QUANTITIES
                // ============================================
                // Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
                // Normalize null values to false
                bool skipValidation = (grParameter?.AllowLessQuantityBasedOnOrder ?? false) 
                    && (grParameter?.AllowMoreQuantityBasedOnOrder ?? false);

                // Normalize RequireAllOrderItemsCollected
                bool requireAllOrderItemsCollected = grParameter?.RequireAllOrderItemsCollected ?? false;

                if (!skipValidation)
                {
                    var lines = await _unitOfWork.GrLines
                        .AsQueryable()
                        .Where(l => l.HeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var line in lines)
                    {
                        // Get total quantity of LineSerials for this Line
                        var totalLineSerialQuantity = await _unitOfWork.GrLineSerials
                            .AsQueryable()
                            .Where(ls => !ls.IsDeleted && ls.LineId == line.Id)
                            .SumAsync(ls => ls.Quantity);

                        // Get total quantity of Routes for ImportLines linked to this Line
                        var totalRouteQuantity = await _unitOfWork.GrRoutes
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
                                var msg = _localizationService.GetLocalizedString("GrHeaderAllOrderItemsMustBeCollected", 
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
                        bool allowLess = grParameter?.AllowLessQuantityBasedOnOrder ?? false;
                        bool allowMore = grParameter?.AllowMoreQuantityBasedOnOrder ?? false;
                        
                        bool quantityMismatch = false;
                        string localizedMessage = string.Empty;
                        string exceptionMessage = string.Empty;

                        if (!allowLess && !allowMore)
                        {
                            // Both false: Exact match required (==)
                            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
                            {
                                quantityMismatch = true;
                                localizedMessage = _localizationService.GetLocalizedString("GrHeaderQuantityExactMatchRequired", 
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
                                localizedMessage = _localizationService.GetLocalizedString("GrHeaderQuantityCannotBeGreater", 
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
                                localizedMessage = _localizationService.GetLocalizedString("GrHeaderQuantityCannotBeLess", 
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
                entity.IsPendingApproval = grParameter != null && grParameter.RequireApprovalBeforeErp;

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

        public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.GrHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.GrTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<GrHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderAssignedOrdersRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.GrLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLines = await _unitOfWork.GrImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                var lineIds = importLines.Where(il => il.LineId.HasValue).Select(il => il.LineId!.Value).Distinct().ToList();
                IEnumerable<GrLineSerial> lineSerials = Array.Empty<GrLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.GrLineSerials
                        .FindAsync(x => x.LineId.HasValue && lineIds.Contains(x.LineId.Value) && !x.IsDeleted);
                }

                IEnumerable<GrRoute> routes = Array.Empty<GrRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.GrRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var importLineDtos = _mapper.Map<IEnumerable<GrImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var dto = new GrAssignedOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<GrLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<GrRouteDto>>(routes)
                };

                return ApiResponse<GrAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.GrHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<GrHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<GrHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<GrHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                // Tracking ile yükle (navigation property'ler yüklenmeyecek)
                var entity = await _unitOfWork.GrHeaders
                    .AsQueryable()
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
                    
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
                    return ApiResponse<GrHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("GrHeaderApprovalUpdateError");
                    return ApiResponse<GrHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.GrHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<GrHeaderDto>(entity);
                
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<GrHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderApprovalUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }


         
    }
}
