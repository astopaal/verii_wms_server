using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrImportLService : IGrImportLService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public GrImportLService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<GrImportLDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _unitOfWork.GrImportLines.AsQueryable();

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "HeaderId":
                        query = desc ? query.OrderByDescending(x => x.HeaderId) : query.OrderBy(x => x.HeaderId);
                        break;
                    case "LineId":
                        query = desc ? query.OrderByDescending(x => x.LineId) : query.OrderBy(x => x.LineId);
                        break;
                    case "StockCode":
                        query = desc ? query.OrderByDescending(x => x.StockCode) : query.OrderBy(x => x.StockCode);
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

                var dtos = _mapper.Map<List<GrImportLDto>>(items);

                var result = new PagedResponse<GrImportLDto>(dtos, totalCount, pageNumber, pageSize);

                return ApiResponse<PagedResponse<GrImportLDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLDto>>> GetAllAsync()
        {
            try
            {
                var grImportLs = await _unitOfWork.GrImportLines.GetAllAsync();
                var grImportLDtos = _mapper.Map<IEnumerable<GrImportLDto>>(grImportLs);
                
                return ApiResponse<IEnumerable<GrImportLDto>>.SuccessResult(
                    grImportLDtos, 
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<GrImportLDto?>> GetByIdAsync(long id)
        {
            try
            {
                var grImportL = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (grImportL == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLNotFound");
                    return ApiResponse<GrImportLDto?>.ErrorResult(
                        nf,
                        nf,
                        404
                    );
                }

                var grImportLDto = _mapper.Map<GrImportLDto>(grImportL);
                
                return ApiResponse<GrImportLDto?>.SuccessResult(
                    grImportLDto,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLDto?>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var grImportLs = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var grImportLDtos = _mapper.Map<IEnumerable<GrImportLDto>>(grImportLs);
                
                return ApiResponse<IEnumerable<GrImportLDto>>.SuccessResult(
                    grImportLDtos,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId)
        {
            try
            {
                var items = await _unitOfWork.GrImportLines
                    .AsQueryable()
                    .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                    .Include(x => x.Routes.Where(r => !r.IsDeleted))
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<GrImportLWithRoutesDto>>(items);

                var enrichedResp = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enrichedResp.Success)
                {
                    return ApiResponse<IEnumerable<GrImportLWithRoutesDto>>.ErrorResult(
                        enrichedResp.Message,
                        enrichedResp.ExceptionMessage,
                        enrichedResp.StatusCode
                    );
                }
                
                dtos = enrichedResp.Data ?? dtos;

                return ApiResponse<IEnumerable<GrImportLWithRoutesDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportLRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var grImportLs = await _unitOfWork.GrImportLines.FindAsync(x => x.LineId == lineId);
                var grImportLDtos = _mapper.Map<IEnumerable<GrImportLDto>>(grImportLs);
                
                return ApiResponse<IEnumerable<GrImportLDto>>.SuccessResult(
                    grImportLDtos,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var grImportLs = await _unitOfWork.GrImportLines.FindAsync(x => x.StockCode == stockCode);
                var grImportLDtos = _mapper.Map<IEnumerable<GrImportLDto>>(grImportLs);
                
                return ApiResponse<IEnumerable<GrImportLDto>>.SuccessResult(
                    grImportLDtos,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<GrImportLDto>> CreateAsync(CreateGrImportLDto createDto)
        {
            try
            {
                var grImportL = _mapper.Map<GrImportLine>(createDto);
                grImportL.CreatedDate = DateTime.UtcNow;
                
                await _unitOfWork.GrImportLines.AddAsync(grImportL);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLDto = _mapper.Map<GrImportLDto>(grImportL);
                
                return ApiResponse<GrImportLDto>.SuccessResult(
                    grImportLDto,
                    _localizationService.GetLocalizedString("GrImportLCreatedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLCreationError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<GrImportLDto>> UpdateAsync(long id, UpdateGrImportLDto updateDto)
        {
            try
            {
                var existingGrImportL = await _unitOfWork.GrImportLines.GetByIdAsync(id);
                
                if (existingGrImportL == null)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLNotFound");
                    return ApiResponse<GrImportLDto>.ErrorResult(
                        nf,
                        nf,
                        404
                    );
                }

                _mapper.Map(updateDto, existingGrImportL);
                existingGrImportL.UpdatedDate = DateTime.UtcNow;
                
                _unitOfWork.GrImportLines.Update(existingGrImportL);
                await _unitOfWork.SaveChangesAsync();
                
                var grImportLDto = _mapper.Map<GrImportLDto>(existingGrImportL);
                
                return ApiResponse<GrImportLDto>.SuccessResult(
                    grImportLDto,
                    _localizationService.GetLocalizedString("GrImportLUpdatedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportLDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLUpdateError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.GrImportLines.ExistsAsync(id);
                if (!exists)
                {
                    var nf = _localizationService.GetLocalizedString("GrImportLNotFound");
                    return ApiResponse<bool>.ErrorResult(
                        nf,
                        nf,
                        404
                    );
                }

                await _unitOfWork.GrImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                
                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("GrImportLDeletedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLDeletionError"),
                    ex.Message,
                    500
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportLDto>>> GetImportLinesByHeaderIdAsync(long headerId)
        {
            try
            {
                var importLines = await _unitOfWork.GrImportLines.FindAsync(x => x.HeaderId == headerId);
                var importLineDtos = _mapper.Map<IEnumerable<GrImportLDto>>(importLines);

                return ApiResponse<IEnumerable<GrImportLDto>>.SuccessResult(
                    importLineDtos,
                    _localizationService.GetLocalizedString("GrImportLRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportLDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GrImportLRetrievalError"),
                    ex.Message,
                    500
                );
            }
        }
        
    }
}
