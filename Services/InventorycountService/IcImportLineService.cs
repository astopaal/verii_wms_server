using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class IcImportLineService : IIcImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public IcImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.IcImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.IcImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<IcImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<IcImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<IcImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.IcImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcImportLineDto>> CreateAsync(CreateIcImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.IcImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcImportLineDto>(entity);
                return ApiResponse<IcImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcImportLineDto>> UpdateAsync(long id, UpdateIcImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.IcImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.IcImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcImportLineDto>(entity);
                return ApiResponse<IcImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.IcImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
                }
                var routes = await _unitOfWork.IcRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("IcImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.IcImportLines.SoftDelete(id);
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineErrorOccurred"), ex.Message, 500);
            }
        }
    }
}
