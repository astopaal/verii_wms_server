using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrLineSerialService : IPrLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.PrLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.PrLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> CreateAsync(CreatePrLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.PrLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 400);
                }
                var entity = _mapper.Map<PrLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.PrLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrLineSerialDto>> UpdateAsync(long id, UpdatePrLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.PrLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineNotFound"), _localizationService.GetLocalizedString("PrLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.PrLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrLineSerialDto>(entity);
                return ApiResponse<PrLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialNotFound"), _localizationService.GetLocalizedString("PrLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.PrLines.GetByIdAsync(entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.PrRoutes
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
                            var msg = _localizationService.GetLocalizedString("PrLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.PrLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.PrRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("PrLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var currentSerialCount = await _unitOfWork.PrLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                var remainingSerialCount = currentSerialCount - 1;

                var hasImportLines = await _unitOfWork.PrImportLines
                    .AsQueryable()
                    .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

                var headerWillBeDeleted = false;
                var headerIdToDelete = 0L;
                if (lineWillBeDeleted && lineEntity != null)
                {
                    var headerId = lineEntity.HeaderId;
                    var currentLinesUnderHeader = await _unitOfWork.PrLines
                        .AsQueryable()
                        .CountAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
                    if (remainingLinesUnderHeader == 0)
                    {
                        var hasHeaderImportLines = await _unitOfWork.PrImportLines
                            .AsQueryable()
                            .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                        if (!hasHeaderImportLines)
                        {
                            headerWillBeDeleted = true;
                            headerIdToDelete = headerId;
                        }
                    }
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.PrLineSerials.SoftDelete(id);

                    if (lineWillBeDeleted)
                    {
                        await _unitOfWork.PrLines.SoftDelete(entity.LineId);
                        if (headerWillBeDeleted && headerIdToDelete != 0)
                        {
                            await _unitOfWork.PrHeaders.SoftDelete(headerIdToDelete);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    var msgKey = lineWillBeDeleted ? "PrLineSerialDeletedAndLineDeleted" : "PrLineSerialDeletedSuccessfully";
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

