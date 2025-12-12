using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrLineSerialService : IGrLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public GrLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.GrLineSerials.AsQueryable().Where(x => !x.IsDeleted);
                sortBy = string.IsNullOrWhiteSpace(sortBy) ? "Id" : sortBy.Trim();
                bool desc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
                query = query.ApplySorting(sortBy, desc);

                var totalCount = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                var dtos = _mapper.Map<List<GrLineSerialDto>>(items);
                var result = new PagedResponse<GrLineSerialDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<GrLineSerialDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var serialLines = await _unitOfWork.GrLineSerials.GetAllAsync();
                var serialLineDtos = _mapper.Map<IEnumerable<GrLineSerialDto>>(serialLines);
                return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(serialLineDtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var serialLine = await _unitOfWork.GrLineSerials.GetByIdAsync(id);
                if (serialLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
                    return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 404);
                }
                var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
                return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineGetByIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var serialLines = await _unitOfWork.GrLineSerials.FindAsync(x => x.ImportLineId == importLineId);
                var serialLineDtos = _mapper.Map<IEnumerable<GrLineSerialDto>>(serialLines);
                return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(serialLineDtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineGetByImportLineIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineSerialDto>> CreateAsync(CreateGrLineSerialDto createDto)
        {
            try
            {
                var importLineExists = await _unitOfWork.GrImportLines.ExistsAsync(createDto.ImportLineId);
                if (!importLineExists)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLNotFound");
                    return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 400);
                }
                var serialLine = _mapper.Map<GrLineSerial>(createDto);
                await _unitOfWork.GrLineSerials.AddAsync(serialLine);
                await _unitOfWork.SaveChangesAsync();
                var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
                return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineCreateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineSerialDto>> UpdateAsync(long id, UpdateGrLineSerialDto updateDto)
        {
            try
            {
                var serialLine = await _unitOfWork.GrLineSerials.GetByIdAsync(id);
                if (serialLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
                    return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 404);
                }
                if (updateDto.ImportLineId.HasValue)
                {
                    var importLineExists = await _unitOfWork.GrImportLines.ExistsAsync(updateDto.ImportLineId.Value);
                    if (!importLineExists)
                    {
                        var nf = _localizationService.GetLocalizedString("GrImportLNotFound");
                        return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 400);
                    }
                    serialLine.ImportLineId = updateDto.ImportLineId.Value;
                }
                _mapper.Map(updateDto, serialLine);
                _unitOfWork.GrLineSerials.Update(serialLine);
                await _unitOfWork.SaveChangesAsync();
                var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
                return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var serialLine = await _unitOfWork.GrLineSerials.GetByIdAsync(id);
                if (serialLine == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                await _unitOfWork.GrLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportSerialLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrImportSerialLineDeleteError"), ex.Message ?? string.Empty, 500);
            }
        }

        
    }
}
