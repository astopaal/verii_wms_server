using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtLineService : ISrtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SrtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.SrtLines.AsQueryable().Where(x => !x.IsDeleted);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
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
                var dtos = _mapper.Map<List<SrtLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                var result = new PagedResponse<SrtLineDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<SrtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtLineNotFound");
                    return ApiResponse<SrtLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtLineDto>(entity);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<SrtLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SrtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.ErpOrderNo == erpOrderNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity)
        {
            try
            {
                var entities = await _unitOfWork.SrtLines.FindAsync(x => x.Quantity >= minQuantity && x.Quantity <= maxQuantity && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtLine>(createDto);
                await _unitOfWork.SrtLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineDto>(entity);
                return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineDto>(entity);
                return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.SrtLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
