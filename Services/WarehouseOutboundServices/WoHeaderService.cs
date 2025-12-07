using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoHeaderService : IWoHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WoHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.WoHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var ascending = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    query = sortBy switch
                    {
                        nameof(WoHeader.ERPReferenceNumber) => ascending ? query.OrderBy(x => x.ERPReferenceNumber) : query.OrderByDescending(x => x.ERPReferenceNumber),
                        nameof(WoHeader.CreatedDate) => ascending ? query.OrderBy(x => x.CreatedDate) : query.OrderByDescending(x => x.CreatedDate),
                        _ => query
                    };
                }

                var totalCount = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<WoHeaderDto>>(items);
                var result = new PagedResponse<WoHeaderDto>(dtos, totalCount, pageNumber, pageSize);
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
                return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.CustomerCode == customerCode && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.DocumentType == documentType && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByOutboundTypeAsync(string outboundType)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.OutboundType == outboundType && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetByAccountCodeAsync(string accountCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WoHeaders.FindAsync(x => x.AccountCode == accountCode && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
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
    }
}
