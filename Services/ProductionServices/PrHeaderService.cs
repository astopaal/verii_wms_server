using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

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
                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
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
   
    }
}
