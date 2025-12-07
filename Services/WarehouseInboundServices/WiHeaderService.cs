using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiHeaderService : IWiHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WiHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.WiHeaders.AsQueryable().Where(x => x.BranchCode == branchCode);

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var ascending = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    query = sortBy switch
                    {
                        nameof(WiHeader.ERPReferenceNumber) => ascending ? query.OrderBy(x => x.ERPReferenceNumber) : query.OrderByDescending(x => x.ERPReferenceNumber),
                        nameof(WiHeader.CreatedDate) => ascending ? query.OrderBy(x => x.CreatedDate) : query.OrderByDescending(x => x.CreatedDate),
                        _ => query
                    };
                }

                var totalCount = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<WiHeaderDto>>(items);
                var result = new PagedResponse<WiHeaderDto>(dtos, totalCount, pageNumber, pageSize);
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
                return ApiResponse<WiHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.CustomerCode == customerCode && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.DocumentType == documentType && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByInboundTypeAsync(string inboundType)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.WiHeaders.FindAsync(x => x.InboundType == inboundType && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(entities);
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
                var existing = await _unitOfWork.WiHeaders.GetByIdAsync(id);
                if (existing == null) return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderNotFound"), _localizationService.GetLocalizedString("WiHeaderNotFound"), 404);
                existing.IsCompleted = true;
                existing.CompletionDate = DateTime.UtcNow;
                _unitOfWork.WiHeaders.Update(existing);
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
                return ApiResponse<IEnumerable<WiHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
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

                var dto = new WiAssignedOrderLinesDto
                {
                    Lines = _mapper.Map<IEnumerable<WiLineDto>>(lines),
                    LineSerials = _mapper.Map<IEnumerable<WiLineSerialDto>>(lineSerials),
                    ImportLines = _mapper.Map<IEnumerable<WiImportLineDto>>(importLines),
                    Routes = _mapper.Map<IEnumerable<WiRouteDto>>(routes)
                };

                return ApiResponse<WiAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiAssignedOrderLinesDto>.ErrorResult(_localizationService.GetLocalizedString("WiHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
