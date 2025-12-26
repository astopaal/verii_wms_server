using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PPackageService : IPPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PPackageService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PPackageDto>>> GetAllAsync()
        {
            try
            {
                var packages = await _unitOfWork.PPackages.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PPackageDto>>(packages);
                return ApiResponse<IEnumerable<PPackageDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PPackageDto>>.ErrorResult(_localizationService.GetLocalizedString("PPackageRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PPackageDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PPackages.AsQueryable();
                query = query.ApplyFilters(request.Filters);

                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<PPackageDto>>(items);
                var result = new PagedResponse<PPackageDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PPackageDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PPackageDto>>.ErrorResult(_localizationService.GetLocalizedString("PPackageRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PPackageDto?>> GetByIdAsync(long id)
        {
            try
            {
                var package = await _unitOfWork.PPackages.GetByIdAsync(id);
                if (package == null)
                {
                    var nf = _localizationService.GetLocalizedString("PPackageNotFound");
                    return ApiResponse<PPackageDto?>.ErrorResult(nf, nf, 404);
                }

                var dto = _mapper.Map<PPackageDto>(package);
                return ApiResponse<PPackageDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PPackageDto?>.ErrorResult(_localizationService.GetLocalizedString("PPackageRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PPackageDto>>> GetByPackingHeaderIdAsync(long packingHeaderId)
        {
            try
            {
                var packages = await _unitOfWork.PPackages.FindAsync(x => x.PackingHeaderId == packingHeaderId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PPackageDto>>(packages);
                return ApiResponse<IEnumerable<PPackageDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PPackageDto>>.ErrorResult(_localizationService.GetLocalizedString("PPackageRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PPackageDto>> CreateAsync(CreatePPackageDto createDto)
        {
            try
            {
                // Validate PackingHeader exists
                var header = await _unitOfWork.PHeaders.GetByIdAsync(createDto.PackingHeaderId);
                if (header == null || header.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("PHeaderNotFound");
                    return ApiResponse<PPackageDto>.ErrorResult(nf, nf, 404);
                }

                var package = _mapper.Map<PPackage>(createDto);
                if (string.IsNullOrWhiteSpace(package.PackageType))
                {
                    package.PackageType = PPackageType.Box;
                }
                if (string.IsNullOrWhiteSpace(package.Status))
                {
                    package.Status = PPackageStatus.Open;
                }

                await _unitOfWork.PPackages.AddAsync(package);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PPackageDto>(package);
                return ApiResponse<PPackageDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PPackageCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PPackageDto>.ErrorResult(_localizationService.GetLocalizedString("PPackageCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PPackageDto>> UpdateAsync(long id, UpdatePPackageDto updateDto)
        {
            try
            {
                var package = await _unitOfWork.PPackages.GetByIdAsync(id);
                if (package == null)
                {
                    var nf = _localizationService.GetLocalizedString("PPackageNotFound");
                    return ApiResponse<PPackageDto>.ErrorResult(nf, nf, 404);
                }

                _mapper.Map(updateDto, package);
                _unitOfWork.PPackages.Update(package);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PPackageDto>(package);
                return ApiResponse<PPackageDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PPackageUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PPackageDto>.ErrorResult(_localizationService.GetLocalizedString("PPackageUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var package = await _unitOfWork.PPackages.GetByIdAsync(id);
                if (package == null)
                {
                    var nf = _localizationService.GetLocalizedString("PPackageNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                await _unitOfWork.PPackages.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PPackageDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PPackageSoftDeletionError"), ex.Message, 500);
            }
        }
    }
}

