using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtHeaderService : IWtHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public WtHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(
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
                var query = _unitOfWork.WtHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "DocumentDate":
                        query = desc ? query.OrderByDescending(x => x.DocumentDate) : query.OrderBy(x => x.DocumentDate);
                        break;
                    case "DocumentNo":
                        query = desc ? query.OrderByDescending(x => x.DocumentNo) : query.OrderBy(x => x.DocumentNo);
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

                var dtos = _mapper.Map<List<WtHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<WtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WtHeaderDto>(dtos, totalCount, pageNumber, pageSize);

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

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => x.DocumentNo == documentNo && !x.IsDeleted && x.BranchCode == branchCode);
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



        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByWarehouseAsync(string warehouse)
        {
            try
            {
                // Filter by SourceWarehouse or TargetWarehouse since WtHeader has these properties
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WtHeaders  
                    .FindAsync(x => (x.SourceWarehouse == warehouse || x.TargetWarehouse == warehouse) && !x.IsDeleted && x.BranchCode == branchCode);
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


        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => x.DocumentDate >= startDate && x.DocumentDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
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

                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                entity.IsPendingApproval = false;

                _unitOfWork.WtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
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

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
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

       public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedTransferOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var headersQuery = _unitOfWork.WtHeaders.AsQueryable();
                var terminalsQuery = _unitOfWork.WtTerminalLines.AsQueryable();

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
 
        public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetCompletedAwaitingErpApprovalAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtHeaders
                    .FindAsync(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);
                var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                var entity = await _unitOfWork.WtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
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
                                var line = new WtLine
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

                                var serial = new WtLineSerial
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
                            await _unitOfWork.WtLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<WtTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                tlines.Add(new WtTerminalLine
                                {
                                    HeaderId = header.Id,
                                    TerminalUserId = t.TerminalUserId
                                });
                            }
                            await _unitOfWork.WtTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

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

        public async Task<ApiResponse<int>> BulkCreateInterWarehouseTransferAsync(BulkCreateWtRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var wtHeader = _mapper.Map<WtHeader>(request.Header);
                        await _unitOfWork.WtHeaders.AddAsync(wtHeader);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<WtLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = new WtLine
                                {
                                    HeaderId = wtHeader.Id,
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
                            foreach (var sDto in request.LineSerials)
                            {
                                long lineId = 0;
                                if (sDto.LineGroupGuid.HasValue)
                                {
                                    var lg = sDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(sDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(sDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineReferenceMissing"), 400);
                                }

                                var serial = new WtLineSerial
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
                            await _unitOfWork.WtLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineGuidToId = new Dictionary<Guid, long>();
                        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var routeGuidToImportLineId = new Dictionary<Guid, long>();

                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<WtImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                long lineId = 0;
                                if (importDto.LineGroupGuid.HasValue)
                                {
                                    var lg = importDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("WtHeaderLineClientKeyNotFound"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("WtHeaderLineReferenceMissing"),
                                        400
                                    );
                                }

                                var importLine = new WtImportLine
                                {
                                    HeaderId = wtHeader.Id,
                                    LineId = lineId,
                                    StockCode = importDto.StockCode
                                };
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.WtImportLines.AddRangeAsync(importLines);
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
                            var routes = new List<WtRoute>(request.Routes.Count);
                            foreach (var rDto in request.Routes)
                            {
                                long lineId = 0;
                                if (rDto.LineGroupGuid.HasValue)
                                {
                                    var lg = rDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("WtHeaderLineGroupGuidNotFound"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(rDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderLineReferenceMissing"), 400);
                                }

                                long importLineId = 0;
                                if (rDto.ImportLineGroupGuid.HasValue)
                                {
                                    var ig = rDto.ImportLineGroupGuid.Value;
                                    if (!importLineGuidToId.TryGetValue(ig, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderRouteGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.ImportLineClientKey))
                                {
                                    if (!importLineKeyToId.TryGetValue(rDto.ImportLineClientKey!, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderRouteGroupGuidNotFound"), 400);
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
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderRouteGroupGuidNotFound"), 400);
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(rDto.ClientKey))
                                    {
                                        if (!routeKeyToImportLineId.TryGetValue(rDto.ClientKey!, out importLineId))
                                        {
                                            await _unitOfWork.RollbackTransactionAsync();
                                            return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderRouteGroupGuidNotFound"), 400);
                                        }
                                    }
                                    else
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("WtHeaderRouteGroupGuidNotFound"), 400);
                                    }
                                }

                                var route = new WtRoute
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

                            await _unitOfWork.WtRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();
                        return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("WtHeaderBulkCreateCompletedSuccessfully"));
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
                return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderBulkCreateError"), combined ?? string.Empty, 500);
            }
        }
    
    }
}
