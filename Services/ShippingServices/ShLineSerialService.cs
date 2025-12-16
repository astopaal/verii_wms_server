using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShLineSerialService : IShLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShLineSerials.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLineSerials.GetByIdAsync(id);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShLineSerialNotFound"); return ApiResponse<ShLineSerialDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShLineSerialDto>(entity);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShLineSerials.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<ShLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> CreateAsync(CreateShLineSerialDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShLineSerial>(createDto);
                var created = await _unitOfWork.ShLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineSerialDto>(created);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShLineSerialDto>> UpdateAsync(long id, UpdateShLineSerialDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShLineSerials.GetByIdAsync(id);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShLineSerialNotFound"); return ApiResponse<ShLineSerialDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShLineSerialDto>(entity);
                return ApiResponse<ShLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShLineSerialNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                var lineEntity = await _unitOfWork.ShLines.GetByIdAsync(id: entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.ShRoutes
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
                            var msg = _localizationService.GetLocalizedString("ShLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.ShLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.ShRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("ShLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var currentSerialCount = await _unitOfWork.ShLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                var remainingSerialCount = currentSerialCount - 1;

                var hasImportLines = await _unitOfWork.ShImportLines
                    .AsQueryable()
                    .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

                var headerWillBeDeleted = false;
                var headerIdToDelete = 0L;
                if (lineWillBeDeleted && lineEntity != null)
                {
                    var headerId = lineEntity.HeaderId;
                    var currentLinesUnderHeader = await _unitOfWork.ShLines
                        .AsQueryable()
                        .CountAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
                    if (remainingLinesUnderHeader == 0)
                    {
                        var hasHeaderImportLines = await _unitOfWork.ShImportLines
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
                    await _unitOfWork.ShLineSerials.SoftDelete(id);

                    if (lineWillBeDeleted)
                    {
                        await _unitOfWork.ShLines.SoftDelete(entity.LineId);
                        if (headerWillBeDeleted && headerIdToDelete != 0)
                        {
                            await _unitOfWork.ShHeaders.SoftDelete(headerIdToDelete);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    var msgKey = lineWillBeDeleted ? "ShLineSerialDeletedAndLineDeleted" : "ShLineSerialDeletedSuccessfully";
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
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

