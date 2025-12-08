using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitHeaderService : ISitHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;

        public SitHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;

                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }

        public async Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.SitHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "BranchCode":
                        query = desc ? query.OrderByDescending(x => x.BranchCode) : query.OrderBy(x => x.BranchCode);
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
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<SitHeaderDto>>(items);

                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data?.ToList() ?? dtos;

                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

                var result = new PagedResponse<SitHeaderDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<SitHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
                    return ApiResponse<SitHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SitHeaderDto>(entity);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
        

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                }
                if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.YearCode?.Length != 4)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.PlannedDate == default)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                var entity = _mapper.Map<SitHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitHeaderDto>(entity);
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitHeaderDto>(entity);
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitHeaders.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
                }
                await _unitOfWork.SitHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                _unitOfWork.SitHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAssignedOrdersAsync(long userId)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var headersQuery = _unitOfWork.SitHeaders.AsQueryable();
                var terminalsQuery = _unitOfWork.SitTerminalLines.AsQueryable();

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
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enrichedCustomer.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
                }
                dtos = enrichedCustomer.Data ?? dtos;
                var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
                if (!enrichedWarehouse.Success)
                {
                    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
                }
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.SitLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var lineIds = lines.Select(l => l.Id).ToList();

                IEnumerable<SitLineSerial> lineSerials = Array.Empty<SitLineSerial>();
                if (lineIds.Count > 0)
                {
                    lineSerials = await _unitOfWork.SitLineSerials
                        .FindAsync(x => lineIds.Contains(x.LineId) && !x.IsDeleted);
                }

                var importLines = await _unitOfWork.SitImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);

                var importLineIds = importLines.Select(il => il.Id).ToList();

                IEnumerable<SitRoute> routes = Array.Empty<SitRoute>();
                if (importLineIds.Count > 0)
                {
                    routes = await _unitOfWork.SitRoutes
                        .FindAsync(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted);
                }

                var dto = new SitAssignedOrderLinesDto
                {
                    Lines = _mapper.Map<IEnumerable<SitLineDto>>(lines),
                    LineSerials = _mapper.Map<IEnumerable<SitLineSerialDto>>(lineSerials),
                    ImportLines = _mapper.Map<IEnumerable<SitImportLineDto>>(importLines),
                    Routes = _mapper.Map<IEnumerable<SitRouteDto>>(routes)
                };

                return ApiResponse<SitAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> GenerateOrderAsync(GenerateSubcontractingIssueOrderRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<SitHeader>(request.Header);
                        await _unitOfWork.SitHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();

                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<SitLine>(request.Lines.Count);
                            foreach (var l in request.Lines)
                            {
                                var line = new SitLine
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
                            await _unitOfWork.SitLines.AddRangeAsync(lines);
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
                            var serials = new List<SitLineSerial>(request.LineSerials.Count);
                            foreach (var s in request.LineSerials)
                            {
                                long lineId = 0;
                                if (s.LineGroupGuid.HasValue)
                                {
                                    var lg = s.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<SitHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<SitHeaderDto>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<SitHeaderDto>.ErrorResult(
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        400
                                    );
                                }

                                var serial = new SitLineSerial
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
                            await _unitOfWork.SitLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
                        {
                            var tlines = new List<SitTerminalLine>(request.TerminalLines.Count);
                            foreach (var t in request.TerminalLines)
                            {
                                tlines.Add(new SitTerminalLine
                                {
                                    HeaderId = header.Id,
                                    TerminalUserId = t.TerminalUserId
                                });
                            }
                            await _unitOfWork.SitTerminalLines.AddRangeAsync(tlines);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        var dto = _mapper.Map<SitHeaderDto>(header);
                        return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderCreatedSuccessfully"));
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
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<int>> BulkCreateSubcontractingIssueTransferAsync(BulkCreateSitRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var header = _mapper.Map<SitHeader>(request.Header);
                        await _unitOfWork.SitHeaders.AddAsync(header);
                        await _unitOfWork.SaveChangesAsync();

                        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var lineGuidToId = new Dictionary<Guid, long>();
                        if (request.Lines != null && request.Lines.Count > 0)
                        {
                            var lines = new List<SitLine>(request.Lines.Count);
                            foreach (var lineDto in request.Lines)
                            {
                                var line = new SitLine
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
                            await _unitOfWork.SitLines.AddRangeAsync(lines);
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
                            var serials = new List<SitLineSerial>(request.LineSerials.Count);
                            foreach (var sDto in request.LineSerials)
                            {
                                long lineId = 0;
                                if (sDto.LineGroupGuid.HasValue)
                                {
                                    var lg = sDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(sDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(sDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        400
                                    );
                                }

                                var serial = new SitLineSerial
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
                            await _unitOfWork.SitLineSerials.AddRangeAsync(serials);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var importLineGuidToId = new Dictionary<Guid, long>();
                        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                        var routeGuidToImportLineId = new Dictionary<Guid, long>();

                        if (request.ImportLines != null && request.ImportLines.Count > 0)
                        {
                            var importLines = new List<SitImportLine>(request.ImportLines.Count);
                            foreach (var importDto in request.ImportLines)
                            {
                                long lineId = 0;
                                if (importDto.LineGroupGuid.HasValue)
                                {
                                    var lg = importDto.LineGroupGuid.Value;
                                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        400
                                    );
                                }

                                var importLine = new SitImportLine
                                {
                                    HeaderId = header.Id,
                                    LineId = lineId,
                                    StockCode = importDto.StockCode
                                };
                                importLines.Add(importLine);
                            }

                            await _unitOfWork.SitImportLines.AddRangeAsync(importLines);
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
                            var routes = new List<SitRoute>(request.Routes.Count);
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
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.LineClientKey))
                                {
                                    if (!lineKeyToId.TryGetValue(rDto.LineClientKey!, out lineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<int>.ErrorResult(
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                        400
                                    );
                                }

                                long importLineId = 0;
                                if (rDto.ImportLineGroupGuid.HasValue)
                                {
                                    var ig = rDto.ImportLineGroupGuid.Value;
                                    if (!importLineGuidToId.TryGetValue(ig, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rDto.ImportLineClientKey))
                                {
                                    if (!importLineKeyToId.TryGetValue(rDto.ImportLineClientKey!, out importLineId))
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
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
                                            return ApiResponse<int>.ErrorResult(
                                                _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                                _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                                400
                                            );
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(rDto.ClientKey))
                                    {
                                        if (!routeKeyToImportLineId.TryGetValue(rDto.ClientKey!, out importLineId))
                                        {
                                            await _unitOfWork.RollbackTransactionAsync();
                                            return ApiResponse<int>.ErrorResult(
                                                _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                                _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                                400
                                            );
                                        }
                                    }
                                    else
                                    {
                                        await _unitOfWork.RollbackTransactionAsync();
                                        return ApiResponse<int>.ErrorResult(
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                                            400
                                        );
                                    }
                                }

                                var route = new SitRoute
                                {
                                    ImportLineId = importLineId,
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

                            await _unitOfWork.SitRoutes.AddRangeAsync(routes);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();
                        return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("OperationSuccessful"));
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
                return ApiResponse<int>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), combined ?? string.Empty, 500);
            }
        }
    }
}
