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

        public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.PrHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "BranchCode":
                        query = desc ? query.OrderByDescending(x => x.BranchCode) : query.OrderBy(x => x.BranchCode);
                        break;
                    case "DocumentType":
                        query = desc ? query.OrderByDescending(x => x.DocumentType) : query.OrderBy(x => x.DocumentType);
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

                var result = new PagedResponse<PrHeaderDto>(dtos, totalCount, pageNumber, pageSize);
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

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.PrHeaders.FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
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

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.PrHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
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

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.PrHeaders.FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted && x.BranchCode == branchCode);
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
                dtos = enrichedWarehouse.Data ?? dtos;
                return ApiResponse<IEnumerable<PrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.PrHeaders.FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
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
    }
}
