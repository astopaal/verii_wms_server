using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SidebarmenuLineService : ISidebarmenuLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SidebarmenuLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SidebarmenuLineDto>>> GetAllAsync()
        {
            try
            {
                var lines = await _unitOfWork.SidebarmenuLines.GetAllAsync();
                var lineDtos = _mapper.Map<IEnumerable<SidebarmenuLineDto>>(lines);
                return ApiResponse<IEnumerable<SidebarmenuLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError");
                return ApiResponse<IEnumerable<SidebarmenuLineDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SidebarmenuLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.SidebarmenuLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<SidebarmenuLineDto>>(items);

                var result = new PagedResponse<SidebarmenuLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<SidebarmenuLineDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SidebarmenuLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }

        public async Task<ApiResponse<SidebarmenuLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.SidebarmenuLines.GetByIdAsync(id);
                if (line == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("SidebarmenuLineNotFound");
                    return ApiResponse<SidebarmenuLineDto>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                var lineDto = _mapper.Map<SidebarmenuLineDto>(line);
                return ApiResponse<SidebarmenuLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError");
                return ApiResponse<SidebarmenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SidebarmenuLineDto>> CreateAsync(CreateSidebarmenuLineDto createDto)
        {
            try
            {
                var line = _mapper.Map<SidebarmenuLine>(createDto);
                var createdLine = await _unitOfWork.SidebarmenuLines.AddAsync(line);
                await _unitOfWork.SaveChangesAsync();
                
                var lineDto = _mapper.Map<SidebarmenuLineDto>(createdLine);
                return ApiResponse<SidebarmenuLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("SidebarmenuLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineCreationError");
                return ApiResponse<SidebarmenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SidebarmenuLineDto>> UpdateAsync(long id, UpdateSidebarmenuLineDto updateDto)
        {
            try
            {
                var existingLine = await _unitOfWork.SidebarmenuLines.GetByIdAsync(id);
                if (existingLine == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("SidebarmenuLineNotFound");
                    return ApiResponse<SidebarmenuLineDto>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                if (updateDto.HeaderId.HasValue)
                    existingLine.HeaderId = updateDto.HeaderId.Value;

                if (updateDto.Page != null)
                    existingLine.Page = updateDto.Page;

                if (updateDto.Title != null)
                    existingLine.Title = updateDto.Title;

                if (updateDto.Description != null)
                    existingLine.Description = updateDto.Description;

                if (updateDto.Icon != null)
                    existingLine.Icon = updateDto.Icon;

                _unitOfWork.SidebarmenuLines.Update(existingLine);
                await _unitOfWork.SaveChangesAsync();

                var lineDto = _mapper.Map<SidebarmenuLineDto>(existingLine);
                return ApiResponse<SidebarmenuLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("SidebarmenuLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineUpdateError");
                return ApiResponse<SidebarmenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.SidebarmenuLines.GetByIdAsync(id);
                if (line == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("SidebarmenuLineNotFound");
                    return ApiResponse<bool>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                await _unitOfWork.SidebarmenuLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SidebarmenuLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("SidebarmenuLineDeletionError"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }

        public async Task<ApiResponse<bool>> ExistsAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SidebarmenuLines.ExistsAsync(id);
                return ApiResponse<bool>.SuccessResult(exists, _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SidebarmenuLineDto>>> GetByHeaderIdAsync(int headerId)
        {
            try
            {
                var lines = await _unitOfWork.SidebarmenuLines.FindAsync(l => l.HeaderId == headerId);
                var lineDtos = _mapper.Map<IEnumerable<SidebarmenuLineDto>>(lines);
                return ApiResponse<IEnumerable<SidebarmenuLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError");
                return ApiResponse<IEnumerable<SidebarmenuLineDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SidebarmenuLineDto>> GetByPageAsync(string page)
        {
            try
            {
                var line = await _unitOfWork.SidebarmenuLines.GetFirstOrDefaultAsync(l => l.Page == page);
                if (line == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("SidebarmenuLineNotFound");
                    return ApiResponse<SidebarmenuLineDto>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                var lineDto = _mapper.Map<SidebarmenuLineDto>(line);
                return ApiResponse<SidebarmenuLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("SidebarmenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("SidebarmenuLineRetrievalError");
                return ApiResponse<SidebarmenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }
    }
}
