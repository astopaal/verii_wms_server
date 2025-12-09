using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PtLineService : IPtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<PtLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.PtLines.AsQueryable().Where(x => !x.IsDeleted);

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
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<PtLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<PtLineDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<PtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }
                var dto = _mapper.Map<PtLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.PtLines.FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineDto>>(entities);
                return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineDto>> CreateAsync(CreatePtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PtLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineDto>(entity);
                return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineDto>> UpdateAsync(long id, UpdatePtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineDto>(entity);
                return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PtLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 404);
                }
                await _unitOfWork.PtLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
