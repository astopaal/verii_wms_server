using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitImportLineService : ISitImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SitImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<SitImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<SitImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.SitImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }





        public async Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitImportLineDto>(entity);
                return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitImportLineDto>(entity);
                return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitImportLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineNotFound"), _localizationService.GetLocalizedString("SitImportLineNotFound"), 404);
                }
                var routes = await _unitOfWork.SitRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("SitImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.SitImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSitImportBarcodeRequestDto request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var header = await _unitOfWork.SitHeaders.GetByIdAsync(request.HeaderId);
                    if (header == null || header.IsDeleted)
                    {
                        var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
                        return ApiResponse<SitImportLineDto>.ErrorResult(nf, nf, 404);
                    }

                    var lineControl = await _unitOfWork.SitImportLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);
                    if (lineControl != null && lineControl.Any())
                    {
                        var lineCounter = lineControl.Count(x => x.StockCode == request.StockCode && x.YapKod == request.YapKod);
                        if (lineCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("SitImportLineStokCodeAndYapCodeNotMatch");
                            return ApiResponse<SitImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    var lineSerialControl = await _unitOfWork.SitLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
                    if (lineSerialControl != null && lineSerialControl.Any())
                    {
                        var lineSerialCounter = lineSerialControl.Count(x => x.SerialNo == request.SerialNo);
                        if (lineSerialCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("SitImportLineSerialNotMatch");
                            return ApiResponse<SitImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(request.SerialNo))
                    {
                        var duplicateExists = await _unitOfWork.SitRoutes.AsQueryable().AnyAsync(r => !r.IsDeleted && r.SerialNo == request.SerialNo && r.ImportLine.HeaderId == request.HeaderId && r.ImportLine.StockCode == request.StockCode && r.ImportLine.YapKod == request.YapKod);
                        if (duplicateExists)
                        {
                            var msg = _localizationService.GetLocalizedString("SitImportLineDuplicateSerialFound");
                            return ApiResponse<SitImportLineDto>.ErrorResult(msg, msg, 409);
                        }
                    }

                    if (request.Quantity <= 0)
                    {
                        var msg = _localizationService.GetLocalizedString("SitImportLineQuantityInvalid");
                        return ApiResponse<SitImportLineDto>.ErrorResult(msg, msg, 400);
                    }

                    SitImportLine? importLine = null;
                    if (request.LineId.HasValue)
                    {
                        importLine = await _unitOfWork.SitImportLines.GetByIdAsync(request.LineId.Value);
                    }
                    else
                    {
                        importLine = (await _unitOfWork.SitImportLines.FindAsync(x => x.HeaderId == request.HeaderId && x.StockCode == request.StockCode && x.YapKod == request.YapKod && !x.IsDeleted)).FirstOrDefault();
                    }

                    if (importLine == null)
                    {
                        importLine = new SitImportLine
                        {
                            HeaderId = request.HeaderId,
                            LineId = request.LineId,
                            StockCode = request.StockCode,
                            YapKod = request.YapKod
                        };
                        await _unitOfWork.SitImportLines.AddAsync(importLine);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var route = new SitRoute
                    {
                        ImportLineId = importLine.Id,
                        ScannedBarcode = request.Barcode,
                        Quantity = request.Quantity,
                        SerialNo = request.SerialNo,
                        SerialNo2 = request.SerialNo2,
                        SerialNo3 = request.SerialNo3,
                        SerialNo4 = request.SerialNo4,
                        SourceCellCode = request.SourceCellCode,
                        TargetCellCode = request.TargetCellCode
                    };

                    await _unitOfWork.SitRoutes.AddAsync(route);
                    await _unitOfWork.SaveChangesAsync();

                    var dto = _mapper.Map<SitImportLineDto>(importLine);
                    scope.Complete();
                    return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineCreatedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<SitImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitImportLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
