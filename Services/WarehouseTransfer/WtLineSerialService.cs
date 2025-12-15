using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtLineSerialService : IWtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> CreateAsync(CreateWtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 400);
                }
                var entity = _mapper.Map<WtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;

                await _unitOfWork.WtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> UpdateAsync(long id, UpdateWtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }

                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 400);
                    }
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;

                _unitOfWork.WtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var tx = await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.WtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }

                if (!string.IsNullOrWhiteSpace(entity.SerialNo))
                {
                    var serialExistsInRoutes = await _unitOfWork.WtRoutes
                        .AsQueryable()
                        .AnyAsync(r => !r.IsDeleted
                                       && r.ImportLine.LineId == entity.LineId
                                       && (
                                           r.SerialNo == entity.SerialNo ||
                                           r.SerialNo2 == entity.SerialNo ||
                                           r.SerialNo3 == entity.SerialNo ||
                                           r.SerialNo4 == entity.SerialNo
                                       ));
                    if (serialExistsInRoutes)
                    {
                        var msg = _localizationService.GetLocalizedString("WtLineSerialRoutesExist");
                        return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                    }
                }

                var totalLineSerialQty = await _unitOfWork.WtLineSerials
                    .AsQueryable()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.WtRoutes
                    .AsQueryable()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("WtLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }


                await _unitOfWork.WtLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                var remainingSerialCount = await _unitOfWork.WtLineSerials
                    .AsQueryable()
                    .CountAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);

                var lineDeleted = false;
                if (remainingSerialCount == 0)
                {
                    var hasImportLines = await _unitOfWork.WtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.LineId == entity.LineId);
                    if (!hasImportLines)
                    {
                        await _unitOfWork.WtLines.SoftDelete(entity.LineId);
                        await _unitOfWork.SaveChangesAsync();
                        lineDeleted = true;
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                var msgKey = lineDeleted ? "WtLineSerialDeletedAndLineDeleted" : "WtLineSerialDeletedSuccessfully";
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
