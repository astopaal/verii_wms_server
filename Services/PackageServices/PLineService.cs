using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PLineService : IPLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetAllAsync()
        {
            try
            {
                var lines = await _unitOfWork.PLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PLines.AsQueryable();
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<PLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                
                var result = new PagedResponse<PLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto?>> GetByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<PLineDto?>.ErrorResult(nf, nf, 404);
                }

                var dto = _mapper.Map<PLineDto>(line);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PLineDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto?>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackageIdAsync(long packageId)
        {
            try
            {
                var lines = await _unitOfWork.PLines.FindAsync(x => x.PackageId == packageId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackingHeaderIdAsync(long packingHeaderId)
        {
            try
            {
                var lines = await _unitOfWork.PLines.FindAsync(x => x.PackingHeaderId == packingHeaderId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PLineDto>>(lines);
                
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PLineRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto>> CreateAsync(CreatePLineDto createDto)
        {
            try
            {
                // Validate PackingHeader exists
                var header = await _unitOfWork.PHeaders.GetByIdAsync(createDto.PackingHeaderId);
                if (header == null || header.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                // Validate Package exists
                var package = await _unitOfWork.PPackages.GetByIdAsync(createDto.PackageId);
                if (package == null || package.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PPackageNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                // Validate Package belongs to PackingHeader
                if (package.PackingHeaderId != createDto.PackingHeaderId)
                {
                    var error = _localizationService.GetLocalizedString("PPackageNotBelongToPHeader");
                    return ApiResponse<PLineDto>.ErrorResult(error, error, 400);
                }

                var line = _mapper.Map<PLine>(createDto);
                await _unitOfWork.PLines.AddAsync(line);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PLineDto>(line);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto>.ErrorResult(_localizationService.GetLocalizedString("PLineCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PLineDto>> UpdateAsync(long id, UpdatePLineDto updateDto)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<PLineDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, line);
                _unitOfWork.PLines.Update(line);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PLineDto>(line);
                var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PLineDto>.ErrorResult(_localizationService.GetLocalizedString("PLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.PLines.GetByIdAsync(id);
                if (line == null)
                {
                    var nf = _localizationService.GetLocalizedString("PLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                // PHeader kontrolü - SourceType ve SourceRouteId kontrolü
                var packingHeader = await _unitOfWork.PHeaders.GetByIdAsync(line.PackingHeaderId);
                bool shouldRetireRoute = false;
                string? sourceType = null;
                long? routeIdToRetire = null;

                if (packingHeader != null && !string.IsNullOrWhiteSpace(packingHeader.SourceType) && line.SourceRouteId.HasValue)
                {
                    shouldRetireRoute = true;
                    sourceType = packingHeader.SourceType;
                    routeIdToRetire = line.SourceRouteId.Value;
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // SourceType'a göre ilgili Route'u retired (soft delete) et
                    if (shouldRetireRoute && routeIdToRetire.HasValue && !string.IsNullOrWhiteSpace(sourceType))
                    {
                        switch (sourceType.ToUpperInvariant())
                        {
                            case PHeaderSourceType.GR:
                                var grRoute = await _unitOfWork.GrRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (grRoute != null && !grRoute.IsDeleted)
                                {
                                    await _unitOfWork.GrRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WT:
                                var wtRoute = await _unitOfWork.WtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (wtRoute != null && !wtRoute.IsDeleted)
                                {
                                    await _unitOfWork.WtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WO:
                                var woRoute = await _unitOfWork.WoRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (woRoute != null && !woRoute.IsDeleted)
                                {
                                    await _unitOfWork.WoRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.WI:
                                var wiRoute = await _unitOfWork.WiRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (wiRoute != null && !wiRoute.IsDeleted)
                                {
                                    await _unitOfWork.WiRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SH:
                                var shRoute = await _unitOfWork.ShRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (shRoute != null && !shRoute.IsDeleted)
                                {
                                    await _unitOfWork.ShRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.PR:
                                var prRoute = await _unitOfWork.PrRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (prRoute != null && !prRoute.IsDeleted)
                                {
                                    await _unitOfWork.PrRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.PT:
                                var ptRoute = await _unitOfWork.PtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (ptRoute != null && !ptRoute.IsDeleted)
                                {
                                    await _unitOfWork.PtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SIT:
                                var sitRoute = await _unitOfWork.SitRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (sitRoute != null && !sitRoute.IsDeleted)
                                {
                                    await _unitOfWork.SitRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            case PHeaderSourceType.SRT:
                                var srtRoute = await _unitOfWork.SrtRoutes.GetByIdAsync(routeIdToRetire.Value);
                                if (srtRoute != null && !srtRoute.IsDeleted)
                                {
                                    await _unitOfWork.SrtRoutes.SoftDelete(routeIdToRetire.Value);
                                }
                                break;

                            default:
                                // Bilinmeyen SourceType için işlem yapılmaz
                                break;
                        }
                    }

                    // PLine'ı sil
                    await _unitOfWork.PLines.SoftDelete(id);
                    
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PLineSoftDeletionError"), ex.Message, 500);
            }
        }

    }
}

