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
    public class SrtHeaderService : ISrtHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public SrtHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.SrtHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<SrtHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<SrtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SrtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(notFound, notFound, 404);
                }
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                }
                if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.YearCode?.Length != 4)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.PlannedDate == default)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
               var entity = _mapper.Map<SrtHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SrtHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SrtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var importLines = await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == id && !x.IsDeleted);
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("SrtHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.SrtHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
                }

                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                entity.IsPendingApproval = false;

                _unitOfWork.SrtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                // Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
                // SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
                // Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
                var query = _unitOfWork.SrtHeaders
                    .AsQueryable()
                    .Where(h => !h.IsDeleted 
                        && !h.IsCompleted 
                        && h.BranchCode == branchCode
                        && _unitOfWork.SrtTerminalLines
                            .AsQueryable()
                            .Any(t => t.HeaderId == h.Id 
                                && !t.IsDeleted 
                                && t.TerminalUserId == userId));

                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderAssignedOrdersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderAssignedOrdersRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.SrtHeaders.AsQueryable()
                    .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();
                var dtos = _mapper.Map<List<SrtHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<SrtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SrtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderCompletedAwaitingErpApprovalRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
        public async Task<ApiResponse<SrtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.SrtLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<SrtLineSerial> lineSerials = Array.Empty<SrtLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.SrtLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.SrtImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<SrtRoute> routes = Array.Empty<SrtRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.SrtRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var lineDtos = _mapper.Map<IEnumerable<SrtLineDto>>(lines);
                if (lineDtos.Any())
                {
                    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
                    if (enrichedLines.Success)
                    {
                        lineDtos = enrichedLines.Data ?? lineDtos;
                    }
                }

                var importLineDtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(importLines);
                if (importLineDtos.Any())
                {
                    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
                    if (enrichedImportLines.Success)
                    {
                        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
                    }
                }

                var dto = new SrtAssignedOrderLinesDto
                {
                    Lines = lineDtos,
                    LineSerials = _mapper.Map<IEnumerable<SrtLineSerialDto>>(lineSerials),
                    ImportLines = importLineDtos,
                    Routes = _mapper.Map<IEnumerable<SrtRouteDto>>(routes)
                };

                return ApiResponse<SrtAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderAssignedOrderLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderAssignedOrderLinesRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<SrtHeaderDto>> SetApprovalAsync(long id, bool approved)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(nf, nf, 404);
                }

                if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
                {
                    var msg = _localizationService.GetLocalizedString("SrtHeaderApprovalUpdateError");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(msg, msg, 400);
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

                _unitOfWork.SrtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderApprovalUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderApprovalUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> GenerateOrderAsync(GenerateSubcontractingReceiptOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<SrtHeader>(request.Header);
                        await _unitOfWork.SrtHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<SrtLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = _mapper.Map<SrtLine>(l);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }
                            await _unitOfWork.SrtLines.AddRangeAsync(lines);
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
                            var serials = new List<SrtLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"), 400);
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"), 400);
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineReferenceMissing"), 400);
                                }

                                var serial = _mapper.Map<SrtLineSerial>(s);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }
                            await _unitOfWork.SrtLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<SrtTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                var tline = _mapper.Map<SrtTerminalLine>(t);
                                tline.HeaderId = header.Id;
                                tlines.Add(tline);
                            }
                            await _unitOfWork.SrtTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        var dto = _mapper.Map<SrtHeaderDto>(header);
                        return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderGenerateCompletedSuccessfully"));
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
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderGenerateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<int>> BulkCreateSubcontractingReceiptTransferAsync(BulkCreateSrtRequestDto request)
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
                        var srtParameter = await _unitOfWork.SrtParameters
                            .AsQueryable()
                            .Where(p => !p.IsDeleted)
                            .FirstOrDefaultAsync();

                        // ============================================
                        // 2. CREATE HEADER
                        // ============================================
                        var header = _mapper.Map<SrtHeader>(request.Header);
                        
                        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
                        header.IsPendingApproval = srtParameter != null && srtParameter.RequireApprovalBeforeErp;

                        await _unitOfWork.SrtHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        if (header?.Id <= 0)
                        {
                            await tx.RollbackAsync();
                            return ApiResponse<int>.ErrorResult(
                                _localizationService.GetLocalizedString("SrtHeaderBulkCreateError"),
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
                            var lines = new List<SrtLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = _mapper.Map<SrtLine>(lineDto);
                                line.HeaderId = header.Id;
                                lines.Add(line);
                            }

                            await _unitOfWork.SrtLines.AddRangeAsync(lines);
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
                            var serials = new List<SrtLineSerial>(request.LineSerials.Count);
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }
                                else
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("LineReferenceMissing"),
                                        400);
                                }

                                var serial = _mapper.Map<SrtLineSerial>(serialDto);
                                serial.LineId = lineId;
                                serials.Add(serial);
                            }

                            await _unitOfWork.SrtLineSerials.AddRangeAsync(serials);
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
                            var importLines = new List<SrtImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                // ClientKey is required for correlation
                                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"),
                                            400);
                                    }
                                    lineId = foundLineId;
                                }

                                var importLine = _mapper.Map<SrtImportLine>(importDto);
                                importLine.HeaderId = header.Id;
                                importLine.LineId = lineId;
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.SrtImportLines.AddRangeAsync(importLines);
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
                            var routes = new List<SrtRoute>(request.Routes.Count);
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
                                            400);
                                    }
                                    importLineId = foundImportLineId;
                                }

                                if (!importLineId.HasValue)
                                {
                                    await tx.RollbackAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                                        _localizationService.GetLocalizedString("SrtHeaderLineReferenceMissing"),
                                        400);
                                }

                                var route = _mapper.Map<SrtRoute>(routeDto);
                                route.ImportLineId = importLineId.Value;
                                routes.Add(route);
                            }

                            await _unitOfWork.SrtRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // ============================================
                        // 7. COMMIT TRANSACTION
                        // ============================================
                        await tx.CommitAsync();
                        return ApiResponse<int>.SuccessResult(
                            1,
                            _localizationService.GetLocalizedString("SrtHeaderBulkCreateCompletedSuccessfully"));
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
                    _localizationService.GetLocalizedString("SrtHeaderBulkCreateError"),
                    combined,
                    500);
            }
        }
    }
}
