using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShLineService : IShLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public ShLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ShLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                var query = _unitOfWork.ShLines.AsQueryable().Where(x => !x.IsDeleted);

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var asc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    query = sortBy switch
                    {
                        nameof(ShLine.HeaderId) => asc ? query.OrderBy(x => x.HeaderId) : query.OrderByDescending(x => x.HeaderId),
                        nameof(ShLine.StockCode) => asc ? query.OrderBy(x => x.StockCode) : query.OrderByDescending(x => x.StockCode),
                        nameof(ShLine.Quantity) => asc ? query.OrderBy(x => x.Quantity) : query.OrderByDescending(x => x.Quantity),
                        _ => query
                    };
                }

                var totalCount = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<ShLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<ShLineDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<ShLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineNotFound");
                    return ApiResponse<ShLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<ShLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<ShLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.ShLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.ShLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.ShLines.FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShLineDto>>(entities);
                return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineDto>> CreateAsync(CreateShLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShLine>(createDto);
                var created = await _unitOfWork.ShLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineDto>(created);
                return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineDto>> UpdateAsync(long id, UpdateShLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShLines.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineNotFound");
                    return ApiResponse<ShLineDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineDto>(entity);
                return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.ShLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
