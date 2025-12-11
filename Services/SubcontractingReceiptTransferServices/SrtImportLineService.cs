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


        public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtImportLines.FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
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
                await _unitOfWork.SrtImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtImportLineDeletedSuccessfully"));
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
                        var lineCounter = lineControl.Count(x => x.StockCode == request.StockCode && x.YapKod == request.YapKod);
                        if (lineCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("SrtImportLineStokCodeAndYapCodeNotMatch");
                            return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    var lineSerialControl = await _unitOfWork.SrtLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
                    if (lineSerialControl != null && lineSerialControl.Any())
                    {
                        var lineSerialCounter = lineSerialControl.Count(x => x.SerialNo == request.SerialNo);
                        if (lineSerialCounter == 0)
                        {
                            var msg = _localizationService.GetLocalizedString("SrtImportLineSerialNotMatch");
                            return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 404);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(request.SerialNo))
                    {
                        var duplicateExists = await _unitOfWork.SrtRoutes.AsQueryable().AnyAsync(r => !r.IsDeleted && r.SerialNo == request.SerialNo && r.ImportLine.HeaderId == request.HeaderId && r.ImportLine.StockCode == request.StockCode && r.ImportLine.YapKod == request.YapKod);
                        if (duplicateExists)
                        {
                            var msg = _localizationService.GetLocalizedString("SrtImportLineDuplicateSerialFound");
                            return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 409);
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
                        importLine = (await _unitOfWork.SrtImportLines.FindAsync(x => x.HeaderId == request.HeaderId && x.StockCode == request.StockCode && x.YapKod == request.YapKod && !x.IsDeleted)).FirstOrDefault();
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
