using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitLineSerialService : ISitLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SitLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.SitLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.SitLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.SitLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
                }
                var entity = _mapper.Map<SitLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.SitLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.SitLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.SitLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                using var tx = await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.SitLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.SitLines.GetByIdAsync(entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.SitRoutes
                            .AsQueryable()
                            .AnyAsync(r => !r.IsDeleted
                                           && r.ImportLine.LineId == entity.LineId
                                           && (
                                               (!string.IsNullOrWhiteSpace(s1) && (r.SerialNo ?? "").Trim() == s1) ||
                                               (!string.IsNullOrWhiteSpace(s2) && (r.SerialNo2 ?? "").Trim() == s2) ||
                                               (!string.IsNullOrWhiteSpace(s3) && (r.SerialNo3 ?? "").Trim() == s3) ||
                                               (!string.IsNullOrWhiteSpace(s4) && (r.SerialNo4 ?? "").Trim() == s4)
                                           ));
                        if (serialExistsInRoutes)
                        {
                            var msg = _localizationService.GetLocalizedString("SitLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.SitLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.SitRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("SitLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                await _unitOfWork.SitLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                var remainingSerialCount = await _unitOfWork.SitLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);

                var lineDeleted = false;
                if (remainingSerialCount == 0)
                {
                    var hasImportLines = await _unitOfWork.SitImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                    if (!hasImportLines)
                    {
                        await _unitOfWork.SitLines.SoftDelete(entity.LineId);
                        await _unitOfWork.SaveChangesAsync();
                        lineDeleted = true;
                        if (lineEntity != null)
                        {
                            var headerId = lineEntity.HeaderId;
                            var remainingLinesUnderHeader = await _unitOfWork.SitLines
                                .AsQueryable()
                                .CountAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                            if (remainingLinesUnderHeader == 0)
                            {
                                var hasHeaderImportLines = await _unitOfWork.SitImportLines
                                    .AsQueryable()
                                    .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                                if (!hasHeaderImportLines)
                                {
                                    await _unitOfWork.SitHeaders.SoftDelete(headerId);
                                    await _unitOfWork.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                var msgKey = lineDeleted ? "SitLineSerialDeletedAndLineDeleted" : "SitLineSerialDeletedSuccessfully";
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
