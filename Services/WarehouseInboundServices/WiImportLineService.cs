using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiImportLineService : IWiImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WiImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiImportLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineNotFound"), _localizationService.GetLocalizedString("WiImportLineNotFound"), 404);
                var dto = _mapper.Map<WiImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<WiImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WiImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WiImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<WiImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WiImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }





        public async Task<ApiResponse<WiImportLineDto>> CreateAsync(CreateWiImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiImportLine>(createDto);
                var created = await _unitOfWork.WiImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiImportLineDto>(created);
                return ApiResponse<WiImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiImportLineDto>> UpdateAsync(long id, UpdateWiImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiImportLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineNotFound"), _localizationService.GetLocalizedString("WiImportLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiImportLineDto>(entity);
                return ApiResponse<WiImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var routes = await _unitOfWork.WiRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("WiImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.WiImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWiImportBarcodeRequestDto request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var header = await _unitOfWork.WiHeaders.GetByIdAsync(request.HeaderId);
                    if (header == null || header.IsDeleted)
                    {
                        var nf = _localizationService.GetLocalizedString("WiHeaderNotFound");
                        return ApiResponse<WiImportLineDto>.ErrorResult(nf, nf, 404);
                    }

                    var lineControl = await _unitOfWork.WiImportLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);
                    if (lineControl != null && lineControl.Any())
                    {
                        var lineCounter = lineControl.Count(x =>
                            ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) &&
                            ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim())
                        );
                        if (lineCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("WiImportLineStokCodeAndYapCodeNotMatch");
                            return ApiResponse<WiImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    var lineSerialControl = await _unitOfWork.WiLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
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
                                var msg = _localizationService.GetLocalizedString("WiImportLineSerialNotMatch");
                                return ApiResponse<WiImportLineDto>.ErrorResult(msg, msg, 404);
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
                            var duplicateExists = await _unitOfWork.WiRoutes.AsQueryable().AnyAsync(r => !r.IsDeleted && r.ImportLine.HeaderId == request.HeaderId && ((r.ImportLine.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((r.ImportLine.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && (((r.SerialNo ?? "").Trim() == s1 && !string.IsNullOrWhiteSpace(s1)) || ((r.SerialNo2 ?? "").Trim() == s2 && !string.IsNullOrWhiteSpace(s2)) || ((r.SerialNo3 ?? "").Trim() == s3 && !string.IsNullOrWhiteSpace(s3)) || ((r.SerialNo4 ?? "").Trim() == s4 && !string.IsNullOrWhiteSpace(s4))));
                            if (duplicateExists)
                            {
                                var msg = _localizationService.GetLocalizedString("WiImportLineDuplicateSerialFound");
                                return ApiResponse<WiImportLineDto>.ErrorResult(msg, msg, 409);
                            }
                        }
                    }

                    if (request.Quantity <= 0)
                    {
                        var msg = _localizationService.GetLocalizedString("WiImportLineQuantityInvalid");
                        return ApiResponse<WiImportLineDto>.ErrorResult(msg, msg, 400);
                    }

                    WiImportLine? importLine = null;
                    if (request.LineId.HasValue)
                    {
                        importLine = await _unitOfWork.WiImportLines.GetByIdAsync(request.LineId.Value);
                    }
                    else
                    {
                        importLine = (await _unitOfWork.WiImportLines.FindAsync(x => x.HeaderId == request.HeaderId && ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && !x.IsDeleted)).FirstOrDefault();
                    }

                    if (importLine == null)
                    {
                        importLine = new WiImportLine
                        {
                            HeaderId = request.HeaderId,
                            LineId = request.LineId,
                            StockCode = request.StockCode,
                            YapKod = request.YapKod
                        };
                        await _unitOfWork.WiImportLines.AddAsync(importLine);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var route = new WiRoute
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

                    await _unitOfWork.WiRoutes.AddAsync(route);
                    await _unitOfWork.SaveChangesAsync();

                    var dto = _mapper.Map<WiImportLineDto>(importLine);
                    scope.Complete();
                    return ApiResponse<WiImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiImportLineCreatedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<WiImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
