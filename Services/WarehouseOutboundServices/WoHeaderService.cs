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
    public class WoHeaderService : IWoHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public WoHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.WoHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WoHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WoHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WoHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoHeaders.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("WoHeaderNotFound"); return ApiResponse<WoHeaderDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<WoHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<WoHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<WoHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        


        

        public async Task<ApiResponse<WoHeaderDto>> CreateAsync(CreateWoHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoHeader>(createDto);
                var created = await _unitOfWork.WoHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoHeaderDto>(created);
                return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoHeaderDto>> UpdateAsync(long id, UpdateWoHeaderDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoHeaders.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("WoHeaderNotFound"); return ApiResponse<WoHeaderDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoHeaderDto>(entity);
                return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.WoHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.WoHeaders.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("WoHeaderNotFound"); return ApiResponse<bool>.ErrorResult(nf, nf, 404); }
                existing.IsCompleted = true;
                existing.CompletionDate = DateTime.UtcNow;
                _unitOfWork.WoHeaders.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var headersQuery = _unitOfWork.WoHeaders.AsQueryable();
                var terminalsQuery = _unitOfWork.WoTerminalLines.AsQueryable();

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
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.WoLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<WoLineSerial> lineSerials = Array.Empty<WoLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.WoLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.WoImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<WoRoute> routes = Array.Empty<WoRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.WoRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var dto = new WoAssignedOrderLinesDto
                {
                    Lines = _mapper.Map<IEnumerable<WoLineDto>>(lines),
                    LineSerials = _mapper.Map<IEnumerable<WoLineSerialDto>>(lineSerials),
                    ImportLines = _mapper.Map<IEnumerable<WoImportLineDto>>(importLines),
                    Routes = _mapper.Map<IEnumerable<WoRouteDto>>(routes)
                };

                return ApiResponse<WoAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.WoHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<WoHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WoHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WoHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                var entity = await _unitOfWork.WoHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("WoHeaderNotFound");
                    return ApiResponse<WoHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("WoHeaderApprovalUpdateError");
                    return ApiResponse<WoHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.WoHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WoHeaderDto>(entity);
                return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoHeaderDto>> GenerateWarehouseOutboundOrderAsync(GenerateWarehouseOutboundOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<WoHeader>(request.Header);
                        await _unitOfWork.WoHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WoLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = new WoLine
                                {
                                    HeaderId = header.Id,
                                    StockCode = l.StockCode,
                                    Quantity = l.Quantity,
                                    Unit = l.Unit,
                                    ErpOrderNo = l.ErpOrderNo,
                                    ErpOrderId = l.ErpOrderId,
                                    Description = l.Description
                                };
                                lines.Add(line);
                            }
                            await _unitOfWork.WoLines.AddRangeAsync(lines);
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
                            var serials = new List<WoLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WoHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                            _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<WoHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                            _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<WoHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                        _localizationService.GetLocalizedString("WoHeaderCreationError"),
                                        400
                                    );
                                }

                                var serial = new WoLineSerial
                                {
                                    LineId = lineId,
                                    Quantity = s.Quantity,
                                    SerialNo = s.SerialNo,
                                    SerialNo2 = s.SerialNo2,
                                    SerialNo3 = s.SerialNo3,
                                    SerialNo4 = s.SerialNo4,
                                    SourceCellCode = s.SourceCellCode,
                                    TargetCellCode = s.TargetCellCode
                                };
                                serials.Add(serial);
                            }
                            await _unitOfWork.WoLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<WoTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                tlines.Add(new WoTerminalLine
                                {
                                    HeaderId = header.Id,
                                    TerminalUserId = t.TerminalUserId
                                });
                            }
                            await _unitOfWork.WoTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        var dto = _mapper.Map<WoHeaderDto>(header);
                        return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderCreatedSuccessfully"));
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
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<int>> BulkCreateWarehouseOutboundAsync(BulkCreateWoRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<WoHeader>(request.Header);
                        await _unitOfWork.WoHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WoLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = new WoLine
                                {
                                    HeaderId = header.Id,
                                    StockCode = lineDto.StockCode,
                                    YapKod = lineDto.YapKod,
                                    Quantity = lineDto.Quantity,
                                    Unit = lineDto.Unit,
                                    ErpOrderNo = lineDto.ErpOrderNo,
                                    ErpOrderId = lineDto.ErpOrderId,
                                    Description = lineDto.Description
                                };
                                lines.Add(line);
                            }
                            await _unitOfWork.WoLines.AddRangeAsync(lines);
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
                            var serials = new List<WoLineSerial>(request.LineSerials.Count);
                            foreach (var sDto in request.LineSerials)
                            {
                                long lineId = 0;
                                if (sDto.LineGroupGuid.HasValue)
                                {
                                    var lg = sDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(sDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(sDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                }

                                var serial = new WoLineSerial
                                {
                                    LineId = lineId,
                                    Quantity = sDto.Quantity,
                                    SerialNo = sDto.SerialNo,
                                    SerialNo2 = sDto.SerialNo2,
                                    SerialNo3 = sDto.SerialNo3,
                                    SerialNo4 = sDto.SerialNo4,
                                    SourceCellCode = sDto.SourceCellCode,
                                    TargetCellCode = sDto.TargetCellCode
                                };
                                serials.Add(serial);
                            }
                            await _unitOfWork.WoLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineGuidToId = new Dictionary<Guid, long>();
                        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var routeGuidToImportLineId = new Dictionary<Guid, long>();

                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<WoImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                long lineId = 0;
                                if (importDto.LineGroupGuid.HasValue)
                                {
                                    var lg = importDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                }

                                var importLine = new WoImportLine
                                {
                                    HeaderId = header.Id,
                                    LineId = lineId,
                                    StockCode = importDto.StockCode
                                };
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.WoImportLines.AddRangeAsync(importLines);
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
                            var routes = new List<WoRoute>(request.Routes.Count);
                            foreach (var rDto in request.Routes)
                            {
                                long lineId = 0;
                                if (rDto.LineGroupGuid.HasValue)
                                {
                                    var lg = rDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(rDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                }

                                long importLineId = 0;
                                if (rDto.ImportLineGroupGuid.HasValue)
                                {
                                    var ig = rDto.ImportLineGroupGuid.Value;
                                    if (!importLineGuidToId.TryGetValue(ig, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.ImportLineClientKey))
                                {
                                    if (!importLineKeyToId.TryGetValue(rDto.ImportLineClientKey!, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
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
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(rDto.ClientKey))
                                    {
                                        if (!routeKeyToImportLineId.TryGetValue(rDto.ClientKey!, out importLineId))
                                        {
                                            await _unitOfWork.RollbackTransactionAsync();
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                        }
                                    }
                                    else
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), _localizationService.GetLocalizedString("WoHeaderCreationError"), 400);
                                    }
                                }

                                var route = new WoRoute
                                {
                                    ImportLineId = importLineId,
                                    LineId = lineId,
                                    Quantity = rDto.Quantity,
                                    SerialNo = rDto.SerialNo,
                                    SerialNo2 = rDto.SerialNo2,
                                    SourceWarehouse = rDto.SourceWarehouse,
                                    TargetWarehouse = rDto.TargetWarehouse,
                                    SourceCellCode = rDto.SourceCellCode,
                                    TargetCellCode = rDto.TargetCellCode,
                                    Description = rDto.Description
                                };
                                routes.Add(route);
                            }

                            await _unitOfWork.WoRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();
                        return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("WoHeaderCreatedSuccessfully"));
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
                return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderCreationError"), combined ?? string.Empty, 500);
            }
        }
    }
}
