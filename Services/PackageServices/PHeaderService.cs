using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PHeaderService : IPHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PHeaderDto>>> GetAllAsync()
        {
            try
            {
                var headers = await _unitOfWork.PHeaders.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PHeaderDto>>(headers);
                
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data ?? dtos;
                
                return ApiResponse<IEnumerable<PHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PHeaders.AsQueryable();
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<PHeaderDto>>(items);
                var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<PHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;
                
                var result = new PagedResponse<PHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto?>> GetByIdAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PHeaderDto?>.ErrorResult(nf, nf, 404);
                }

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto?>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto?>.ErrorResult(_localizationService.GetLocalizedString("PHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto>> CreateAsync(CreatePHeaderDto createDto)
        {
            try
            {
                // PackingNo unique kontrolü - silinmemiş kayıt varsa hata döndür
                if (!string.IsNullOrWhiteSpace(createDto.PackingNo))
                {
                    var existingHeader = await _unitOfWork.PHeaders
                        .AsQueryable()
                        .FirstOrDefaultAsync(h => !h.IsDeleted && h.PackingNo == createDto.PackingNo);

                    if (existingHeader != null)
                    {
                        var errorMessage = _localizationService.GetLocalizedString("PHeaderPackingNoAlreadyExists") 
                            ?? $"Packing number '{createDto.PackingNo}' already exists";
                        return ApiResponse<PHeaderDto>.ErrorResult(errorMessage, errorMessage, 400);
                    }
                }

                var header = _mapper.Map<PHeader>(createDto);
                if (string.IsNullOrWhiteSpace(header.Status))
                {
                    header.Status = PHeaderStatus.Draft;
                }

                await _unitOfWork.PHeaders.AddAsync(header);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PHeaderCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PHeaderDto>> UpdateAsync(long id, UpdatePHeaderDto updateDto)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PHeaderDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, header);
                _unitOfWork.PHeaders.Update(header);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PHeaderDto>(header);
                var enriched = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
                if (!enriched.Success)
                {
                    return ApiResponse<PHeaderDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dto = enriched.Data?.FirstOrDefault() ?? dto;
                
                return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("PHeaderUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                await _unitOfWork.PHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PHeaderSoftDeletionError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<object>>> GetAvailableHeadersForMappingAsync(string sourceType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceType))
                {
                    return ApiResponse<IEnumerable<object>>.ErrorResult(
                        _localizationService.GetLocalizedString("InvalidSourceType") ?? "Source type is required",
                        "Source type is required",
                        400);
                }

                // PHeader tablosunda bu SourceType ile eşleşmiş header ID'lerini al
                var mappedHeaderIds = await _unitOfWork.PHeaders
                    .AsQueryable()
                    .Where(ph => !ph.IsDeleted && ph.SourceType == sourceType && ph.SourceHeaderId.HasValue)
                    .Select(ph => ph.SourceHeaderId!.Value)
                    .ToListAsync();

                // SourceType'a göre ilgili header'ları çek ve eşleşmemiş olanları döndür
                IEnumerable<object> result = sourceType.ToUpperInvariant() switch
                {
                    PHeaderSourceType.GR => await GetAvailableGrHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WT => await GetAvailableWtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SH => await GetAvailableShHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.PR => await GetAvailablePrHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.PT => await GetAvailablePtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SIT => await GetAvailableSitHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.SRT => await GetAvailableSrtHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WI => await GetAvailableWiHeadersAsync(mappedHeaderIds),
                    PHeaderSourceType.WO => await GetAvailableWoHeadersAsync(mappedHeaderIds),
                    _ => Array.Empty<object>()
                };

                return ApiResponse<IEnumerable<object>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully") ?? "Available headers retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<object>>.ErrorResult(
                    _localizationService.GetLocalizedString("AvailableHeadersRetrievalError") ?? "Error occurred while retrieving available headers",
                    ex.Message,
                    500);
            }
        }

        private async Task<IEnumerable<object>> GetAvailableGrHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.GrHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<GrHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WtHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableShHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.ShHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ShHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailablePrHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.PrHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PrHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailablePtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.PtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PtHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableSitHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.SitHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableSrtHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.SrtHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(headers);
            return dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWiHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WiHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WiHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }

        private async Task<IEnumerable<object>> GetAvailableWoHeadersAsync(List<long> mappedHeaderIds)
        {
            var headers = await _unitOfWork.WoHeaders
                .AsQueryable()
                .Where(h => !h.IsDeleted && !mappedHeaderIds.Contains(h.Id))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<WoHeaderDto>>(headers);
            var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
            return enriched.Data?.Cast<object>() ?? dtos.Cast<object>();
        }
    }
}

