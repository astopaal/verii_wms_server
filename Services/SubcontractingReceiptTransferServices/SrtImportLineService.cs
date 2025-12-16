using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtImportLineService : ISrtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public SrtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }




        public async Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SrtImportLine>(createDto);
                await _unitOfWork.SrtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtImportLineDto>(entity);
                return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var routes = await _unitOfWork.SrtRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("SrtImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var hasActiveLineSerials = await _unitOfWork.SrtLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("SrtImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.SrtImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.SrtLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.SrtImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.SrtHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var header = await _unitOfWork.SrtHeaders.GetByIdAsync(request.HeaderId);
                    if (header == null || header.IsDeleted)
                    {
                        var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                        return ApiResponse<SrtImportLineDto>.ErrorResult(nf, nf, 404);
                    }

                    var lineControl = await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);
                    if (lineControl != null && lineControl.Any())
                    {
                        var reqStock = (request.StockCode ?? "").Trim();
                        var reqYap = (request.YapKod ?? "").Trim();
                        var lineCounter = lineControl.Count(x =>
                            ((x.StockCode ?? "").Trim() == reqStock) && ((x.YapKod ?? "").Trim() == reqYap)
                        );
                        if (lineCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("SrtImportLineStokCodeAndYapCodeNotMatch");
                            return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    var lineSerialControl = await _unitOfWork.SrtLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
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
                                var msg = _localizationService.GetLocalizedString("SrtImportLineSerialNotMatch");
                                return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 404);
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
                            var duplicateExists = await _unitOfWork.SrtRoutes.AsQueryable().AnyAsync(r => !r.IsDeleted && r.ImportLine.HeaderId == request.HeaderId && ((r.ImportLine.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((r.ImportLine.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && (((r.SerialNo ?? "").Trim() == s1 && !string.IsNullOrWhiteSpace(s1)) || ((r.SerialNo2 ?? "").Trim() == s2 && !string.IsNullOrWhiteSpace(s2)) || ((r.SerialNo3 ?? "").Trim() == s3 && !string.IsNullOrWhiteSpace(s3)) || ((r.SerialNo4 ?? "").Trim() == s4 && !string.IsNullOrWhiteSpace(s4))));
                            if (duplicateExists)
                            {
                                var msg = _localizationService.GetLocalizedString("SrtImportLineDuplicateSerialFound");
                                return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 409);
                            }
                        }
                    }

                    if (request.Quantity <= 0)
                    {
                        var msg = _localizationService.GetLocalizedString("SrtImportLineQuantityInvalid");
                        return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 400);
                    }

                    SrtImportLine? importLine = null;
                    if (request.LineId.HasValue)
                    {
                        importLine = await _unitOfWork.SrtImportLines.GetByIdAsync(request.LineId.Value);
                    }
                    else
                    {
                        importLine = (await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == request.HeaderId && ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim()) && ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim()) && !x.IsDeleted)).FirstOrDefault();
                    }

                    if (importLine == null)
                    {
                        importLine = new SrtImportLine
                        {
                            HeaderId = request.HeaderId,
                            LineId = request.LineId,
                            StockCode = request.StockCode,
                            YapKod = request.YapKod
                        };
                        await _unitOfWork.SrtImportLines.AddAsync(importLine);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var route = new SrtRoute
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

                    await _unitOfWork.SrtRoutes.AddAsync(route);
                    await _unitOfWork.SaveChangesAsync();

                    var dto = _mapper.Map<SrtImportLineDto>(importLine);
                    scope.Complete();
                    return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
