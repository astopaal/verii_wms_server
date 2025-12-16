using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtLineSerialService : ISrtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SrtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.SrtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.SrtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> CreateAsync(CreateSrtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.SrtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 400);
                }
                var entity = _mapper.Map<SrtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.SrtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> UpdateAsync(long id, UpdateSrtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.SrtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.SrtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                using var tx = await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.SrtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.SrtLines.GetByIdAsync(entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.SrtRoutes
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
                            var msg = _localizationService.GetLocalizedString("SrtLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.SrtLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.SrtRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("SrtLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                await _unitOfWork.SrtLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                var remainingSerialCount = await _unitOfWork.SrtLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);

                var lineDeleted = false;
                if (remainingSerialCount == 0)
                {
                    var hasImportLines = await _unitOfWork.SrtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                    if (!hasImportLines)
                    {
                        await _unitOfWork.SrtLines.SoftDelete(entity.LineId);
                        await _unitOfWork.SaveChangesAsync();
                        lineDeleted = true;
                        if (lineEntity != null)
                        {
                            var headerId = lineEntity.HeaderId;
                            var remainingLinesUnderHeader = await _unitOfWork.SrtLines
                                .AsQueryable()
                                .CountAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                            if (remainingLinesUnderHeader == 0)
                            {
                                var hasHeaderImportLines = await _unitOfWork.SrtImportLines
                                    .AsQueryable()
                                    .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                                if (!hasHeaderImportLines)
                                {
                                    await _unitOfWork.SrtHeaders.SoftDelete(headerId);
                                    await _unitOfWork.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                var msgKey = lineDeleted ? "SrtLineSerialDeletedAndLineDeleted" : "SrtLineSerialDeletedSuccessfully";
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
