using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrLineService : IGrLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public GrLineService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<GrLineDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.GrLines.AsQueryable();

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "HeaderId":
                        query = desc ? query.OrderByDescending(x => x.HeaderId) : query.OrderBy(x => x.HeaderId);
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

                var dtos = _mapper.Map<List<GrLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;

                var result = new PagedResponse<GrLineDto>(dtos, totalCount, pageNumber, pageSize);

                return ApiResponse<PagedResponse<GrLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetAllAsync()
        {
            try
            {
                var lines = await _unitOfWork.GrLines.GetAllAsync();
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.GrLines.GetByIdAsync(id);
                if (line == null)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineNotFound"), "Record not found", 404, "GrLine not found");
                }

                var lineDto = _mapper.Map<GrLineDto>(line);

                var enriched = await _erpService.PopulateStockNamesAsync(new[] { lineDto });
                if (!enriched.Success)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? lineDto;

                return ApiResponse<GrLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.GrLines.FindAsync(x => x.HeaderId == headerId);
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByHeaderIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> CreateAsync(CreateGrLineDto createDto)
        {
            try
            {
                // Header'ın var olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)createDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderNotFound"), "Header not found", 400, "GrHeader not found");
                }

                var line = _mapper.Map<GrLine>(createDto);

                await _unitOfWork.GrLines.AddAsync(line);
                await _unitOfWork.SaveChangesAsync();

                var lineDto = _mapper.Map<GrLineDto>(line);
                return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineCreateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> UpdateAsync(long id, UpdateGrLineDto updateDto)
        {
            try
            {
                var existingLine = await _unitOfWork.GrLines.GetByIdAsync(id);
                if (existingLine == null)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineNotFound"), "Record not found", 404, "GrLine not found");
                }

                // Header'ın var olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)updateDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderNotFound"), "Header not found", 400, "GrHeader not found");
                }

                _mapper.Map(updateDto, existingLine);

                _unitOfWork.GrLines.Update(existingLine);
                await _unitOfWork.SaveChangesAsync();

                var lineDto = _mapper.Map<GrLineDto>(existingLine);
                return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.GrLines.GetByIdAsync(id);
                if (line == null)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrLineNotFound"), "Record not found", 404, "GrLine not found");
                }

                await _unitOfWork.GrLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrLineDeleteError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> ExistsAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.GrLines.ExistsAsync((int)id);
                return ApiResponse<bool>.SuccessResult(exists, _localizationService.GetLocalizedString("GrLineExistsCheckCompleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrLineExistsError"), ex.Message, 500);
            }
        }

        // GrHeader ilişkili satırları (GrLine) headerId’ye göre getirir
        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetLinesByHeaderIdAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.GrLines.FindAsync(x => x.HeaderId == headerId);
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByHeaderIdError"), ex.Message, 500);
            }
        }
        
    }
}
