using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiLineSerialService : IWiLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WiLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WiLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                }
                var entity = _mapper.Map<WiLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.WiLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WiLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.WiLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                using var tx = await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.WiLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.WiLines.GetByIdAsync(entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.WiRoutes
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
                            var msg = _localizationService.GetLocalizedString("WiLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.WiLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.WiRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("WiLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                await _unitOfWork.WiLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                var remainingSerialCount = await _unitOfWork.WiLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);

                var lineDeleted = false;
                if (remainingSerialCount == 0)
                {
                    var hasImportLines = await _unitOfWork.WiImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                    if (!hasImportLines)
                    {
                        await _unitOfWork.WiLines.SoftDelete(entity.LineId);
                        await _unitOfWork.SaveChangesAsync();
                        lineDeleted = true;
                        if (lineEntity != null)
                        {
                            var headerId = lineEntity.HeaderId;
                            var remainingLinesUnderHeader = await _unitOfWork.WiLines
                                .AsQueryable()
                                .CountAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                            if (remainingLinesUnderHeader == 0)
                            {
                                var hasHeaderImportLines = await _unitOfWork.WiImportLines
                                    .AsQueryable()
                                    .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                                if (!hasHeaderImportLines)
                                {
                                    await _unitOfWork.WiHeaders.SoftDelete(headerId);
                                    await _unitOfWork.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                var msgKey = lineDeleted ? "WiLineSerialDeletedAndLineDeleted" : "WiLineSerialDeletedSuccessfully";
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
