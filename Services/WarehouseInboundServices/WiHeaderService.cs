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

        public WiHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
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

                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                entity.IsPendingApproval = false;

                _unitOfWork.WiHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiHeaderCompletedSuccessfully"));
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
                var headersQuery = _unitOfWork.WiHeaders.AsQueryable();
                var terminalsQuery = _unitOfWork.WiTerminalLines.AsQueryable();

                var query = headersQuery
                    .Join(
                        terminalsQuery,
                        h => h.Id,
                        t => t.HeaderId,
                        (h, t) => new { h, t }
                    )
                    .Where(x => !x.h.IsDeleted && !x.h.IsCompleted && !x.t.IsDeleted && x.t.TerminalUserId == userId && x.h.BranchCode == branchCode)
                    .Select(x => x.h)
                    .Distinct();

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
                var entity = await _unitOfWork.WiHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
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
                        }

                        await _unitOfWork.CommitTransactionAsync();

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
                        var header = _mapper.Map<WiHeader>(request.Header);
                        await _unitOfWork.WiHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

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
                            foreach (var sDto in request.LineSerials)
                            {
                                long lineId = 0;
                                if (sDto.LineGroupGuid.HasValue)
                                {
                                    var lg = sDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(sDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(sDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                }

                                var serial = _mapper.Map<WiLineSerial>(sDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.WiLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineGuidToId = new Dictionary<Guid, long>();
                        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var routeGuidToImportLineId = new Dictionary<Guid, long>();

                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<WiImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                long lineId = 0;
                                if (importDto.LineGroupGuid.HasValue)
                                {
                                    var lg = importDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                }

                                var importLine = _mapper.Map<WiImportLine>(importDto);
                                importLine.HeaderId = header.Id;
                                importLine.LineId = lineId;
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.WiImportLines.AddRangeAsync(importLines);
                            await _unitOfWork.SaveChangesAsync();

                            for (int i = 0; i < request.ImportLines.Count; i++)
                            {
                                var id = importLines[i].Id;
                                var key1 = request.ImportLines[i].ClientKey;
                                var guid1 = request.ImportLines[i].ClientGroupGuid;
                                if (!string.IsNullOrWhiteSpace(key1))
                                {
                                    importLineKeyToId[key1!] = id;
                                }
                                if (guid1.HasValue)
                                {
                                    importLineGuidToId[guid1.Value] = id;
                                }

                                var key2 = request.ImportLines[i].RouteClientKey;
                                var guid2 = request.ImportLines[i].RouteGroupGuid;
                                if (!string.IsNullOrWhiteSpace(key2))
                                {
                                    routeKeyToImportLineId[key2!] = id;
                                }
                                if (guid2.HasValue)
                                {
                                    routeGuidToImportLineId[guid2.Value] = id;
                                }
                            }
                        }

                        if (request.Routes != null && request.Routes.Count > 0)
                        {
                            var routes = new List<WiRoute>(request.Routes.Count);
                            foreach (var rDto in request.Routes)
                            {
                                long lineId = 0;
                                if (rDto.LineGroupGuid.HasValue)
                                {
                                    var lg = rDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(rDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                }

                                long importLineId = 0;
                                if (rDto.ImportLineGroupGuid.HasValue)
                                {
                                    var ig = rDto.ImportLineGroupGuid.Value;
                                    if (!importLineGuidToId.TryGetValue(ig, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.ImportLineClientKey))
                                {
                                    if (!importLineKeyToId.TryGetValue(rDto.ImportLineClientKey!, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }
                                else
                                {
                                    if (rDto.ClientGroupGuid.HasValue)
                                    {
                                        var rg = rDto.ClientGroupGuid.Value;
                                        if (!routeGuidToImportLineId.TryGetValue(rg, out importLineId))
                                        {
                                            await _unitOfWork.RollbackTransactionAsync();
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(rDto.ClientKey))
                                    {
                                        if (!routeKeyToImportLineId.TryGetValue(rDto.ClientKey!, out importLineId))
                                        {
                                            await _unitOfWork.RollbackTransactionAsync();
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                        }
                                    }
                                    else
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), _localizationService.GetLocalizedString("WiHeaderErrorOccurred"), 400);
                                    }
                                }

                                var route = _mapper.Map<WiRoute>(rDto);
                                route.ImportLineId = importLineId;
                                routes.Add(route);
                            }

                            await _unitOfWork.WiRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();
                        return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("WiHeaderCreatedSuccessfully"));
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
                var inner = ex.InnerException?.Message ?? string.Empty;
                var combined = string.IsNullOrWhiteSpace(inner) ? ex.Message : ($"{ex.Message} | Inner: {inner}");
                return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), combined ?? String.Empty, 500);
            }
        }
  
    }
}
