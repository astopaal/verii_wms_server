using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtLineService : IWtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<WtLineDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.WtLines.AsQueryable()
                    .Where(x => !x.IsDeleted);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "HeaderId":
                        query = desc ? query.OrderByDescending(x => x.HeaderId) : query.OrderBy(x => x.HeaderId);
                        break;
                    case "StockCode":
                        query = desc ? query.OrderByDescending(x => x.StockCode) : query.OrderBy(x => x.StockCode);
                        break;
                    case "Quantity":
                        query = desc ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity);
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

                var dtos = _mapper.Map<List<WtLineDto>>(items);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<PagedResponse<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data?.ToList() ?? dtos;

                var result = new PagedResponse<WtLineDto>(dtos, totalCount, pageNumber, pageSize);

                return ApiResponse<PagedResponse<WtLineDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WtLineErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                var dto = _mapper.Map<WtLineDto>(entity);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedStock.Success)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                var finalDto = enrichedStock.Data?.FirstOrDefault() ?? dto;

                return ApiResponse<WtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                // WtLine model doesn't have SerialNo property, filtering only by IsDeleted
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByWarehouseAsync(string warehouse)
        {
            try
            {
                // WtLine model doesn't have Warehouse property, filtering only by IsDeleted
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

                var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedStock.Success)
                {
                    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                dtos = enrichedStock.Data ?? dtos;

                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> CreateAsync(CreateWtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;

                await _unitOfWork.WtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineDto>(entity);
                return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineDto>> UpdateAsync(long id, UpdateWtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.WtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineDto>(entity);
                return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WtLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
                }

                await _unitOfWork.WtLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.WtLines
                    .FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);   
                return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
