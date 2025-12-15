using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoImportLineService : IWoImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WoImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoImportLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineNotFound"), _localizationService.GetLocalizedString("WoImportLineNotFound"), 404);
                var dto = _mapper.Map<WoImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<WoImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WoImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WoImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<WoImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WoImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }





        public async Task<ApiResponse<WoImportLineDto>> CreateAsync(CreateWoImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WoImportLine>(createDto);
                var created = await _unitOfWork.WoImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoImportLineDto>(created);
                return ApiResponse<WoImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoImportLineDto>> UpdateAsync(long id, UpdateWoImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WoImportLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineNotFound"), _localizationService.GetLocalizedString("WoImportLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WoImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoImportLineDto>(entity);
                return ApiResponse<WoImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var routes = await _unitOfWork.WoRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WoImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.WoImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWoImportBarcodeRequestDto request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var header = await _unitOfWork.WoHeaders.GetByIdAsync(request.HeaderId);
                    if (header == null || header.IsDeleted)
                    {
                        var nf = _localizationService.GetLocalizedString("WoHeaderNotFound");
                        return ApiResponse<WoImportLineDto>.ErrorResult(nf, nf, 404);
                    }

                    var lineControl = await _unitOfWork.WoImportLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);
                    if (lineControl != null && lineControl.Any())
                    {
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        var lineCounter = lineControl.Count(x =>
                            ((x.StockCode ?? "").Trim() == reqStock) && ((x.YapKod ?? "").Trim() == reqYap)
                        );
                        if (lineCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("WoImportLineStokCodeAndYapCodeNotMatch");
                            return ApiResponse<WoImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    var lineSerialControl = await _unitOfWork.WoLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
                    if (lineSerialControl != null && lineSerialControl.Any())
                    {
                        var s1 = (request.SerialNo ?? "").Trim();
                        var s2 = (request.SerialNo2 ?? "").Trim();
                        var s3 = (request.SerialNo3 ?? "").Trim();
                        var s4 = (request.SerialNo4 ?? "").Trim();
                        var anyReqSerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                        if (anyReqSerial)
                        {
                            var lineSerialCounter = lineSerialControl.Count(x =>
                                (!string.IsNullOrWhiteSpace(s1) && ((x.SerialNo ?? "").Trim() == s1)) ||
                                (!string.IsNullOrWhiteSpace(s2) && ((x.SerialNo2 ?? "").Trim() == s2)) ||
                                (!string.IsNullOrWhiteSpace(s3) && ((x.SerialNo3 ?? "").Trim() == s3)) ||
                                (!string.IsNullOrWhiteSpace(s4) && ((x.SerialNo4 ?? "").Trim() == s4))
                            );
                            if (lineSerialCounter == 0)
                            {
                                var msg = _localizationService.GetLocalizedString("WoImportLineSerialNotMatch");
                                return ApiResponse<WoImportLineDto>.ErrorResult(msg, msg, 404);
                            }
                        }
                    }

                    {
                        var s1 = (request.SerialNo ?? "").Trim();
                        var s2 = (request.SerialNo2 ?? "").Trim();
                        var s3 = (request.SerialNo3 ?? "").Trim();
                        var s4 = (request.SerialNo4 ?? "").Trim();
                        var anyReqSerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                        if (anyReqSerial)
                        {
                            var duplicateExists = await _unitOfWork.WoRoutes.AsQueryable().AnyAsync(r => !r.IsDeleted && r.ImportLine.HeaderId == request.HeaderId && ((r.ImportLine.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((r.ImportLine.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && (((r.SerialNo ?? "").Trim() == s1 && !string.IsNullOrWhiteSpace(s1)) || ((r.SerialNo2 ?? "").Trim() == s2 && !string.IsNullOrWhiteSpace(s2)) || ((r.SerialNo3 ?? "").Trim() == s3 && !string.IsNullOrWhiteSpace(s3)) || ((r.SerialNo4 ?? "").Trim() == s4 && !string.IsNullOrWhiteSpace(s4))));
                            if (duplicateExists)
                            {
                                var msg = _localizationService.GetLocalizedString("WoImportLineDuplicateSerialFound");
                                return ApiResponse<WoImportLineDto>.ErrorResult(msg, msg, 409);
                            }
                        }
                    }

                    if (request.Quantity <= 0)
                    {
                        var msg = _localizationService.GetLocalizedString("WoImportLineQuantityInvalid");
                        return ApiResponse<WoImportLineDto>.ErrorResult(msg, msg, 400);
                    }

                    WoImportLine? importLine = null;
                    if (request.LineId.HasValue)
                    {
                        importLine = await _unitOfWork.WoImportLines.GetByIdAsync(request.LineId.Value);
                    }
                    else
                    {
                        importLine = (await _unitOfWork.WoImportLines.FindAsync(x => x.HeaderId == request.HeaderId && ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && !x.IsDeleted)).FirstOrDefault();
                    }

                    if (importLine == null)
                    {
                        importLine = new WoImportLine
                        {
                            HeaderId = request.HeaderId,
                            LineId = request.LineId,
                            StockCode = request.StockCode,
                            YapKod = request.YapKod
                        };
                        await _unitOfWork.WoImportLines.AddAsync(importLine);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var route = new WoRoute
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

                    await _unitOfWork.WoRoutes.AddAsync(route);
                    await _unitOfWork.SaveChangesAsync();

                    var dto = _mapper.Map<WoImportLineDto>(importLine);
                    scope.Complete();
                    return ApiResponse<WoImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoImportLineCreatedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<WoImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
