using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Transactions;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtImportLineService : IWtImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public WtImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtImportLines
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineNotFound"), _localizationService.GetLocalizedString("WtImportLineNotFound"), 404);
                }

                var dto = _mapper.Map<WtImportLineDto>(entity);
                var enrichedStock = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedStock.Success)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
                }
                var finalDto = enrichedStock.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<WtImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.StockCode == stockCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }



        // Quantity alanı WtImportLine modelinde bulunmuyor, bu nedenle kapsam dışında bırakıldı

        public async Task<ApiResponse<WtImportLineDto>> CreateAsync(CreateWtImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WtImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;

                await _unitOfWork.WtImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtImportLineDto>(entity);
                return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> UpdateAsync(long id, UpdateWtImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineNotFound"), _localizationService.GetLocalizedString("WtImportLineNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.WtImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtImportLineDto>(entity);
                return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WtImportLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineNotFound"), _localizationService.GetLocalizedString("WtImportLineNotFound"), 404);
                }

                await _unitOfWork.WtImportLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtImportLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByRouteIdAsync(long routeId)
        {
            try
            {
                var routes = await _unitOfWork.WtRoutes.FindAsync(r => r.Id == routeId && !r.IsDeleted);
                var importLineIds = routes.Select(r => r.ImportLineId).ToList();

                var entities = importLineIds.Count == 0
                    ? new List<WtImportLine>()
                    : await _unitOfWork.WtImportLines.FindAsync(x => importLineIds.Contains(x.Id) && !x.IsDeleted);

                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.WtHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderNotFound"), _localizationService.GetLocalizedString("WtHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.WtImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var items = new List<WtImportLineWithRoutesDto>();

                foreach (var il in importLines)
                {
                    var routes = await _unitOfWork.WtRoutes.FindAsync(r => r.ImportLineId == il.Id && !r.IsDeleted);
                    var dto = new WtImportLineWithRoutesDto
                    {
                        ImportLine = _mapper.Map<WtImportLineDto>(il),
                        Routes = _mapper.Map<List<WtRouteDto>>(routes)
                    };
                    items.Add(dto);
                }

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(
                        enriched.Message,
                        enriched.ExceptionMessage,
                        enriched.StatusCode
                    );
                }
                var enrichedList = enriched.Data?.ToList() ?? importLineDtos;
                for (int i = 0; i < items.Count && i < enrichedList.Count; i++)
                {
                    items[i].ImportLine = enrichedList[i];
                }

                return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByErpOrderNoAsync(string erpOrderNo)
        {
            try
            {
                var lines = await _unitOfWork.WtLines.FindAsync(l => l.ErpOrderNo == erpOrderNo && !l.IsDeleted);
                var lineIds = lines.Select(l => l.Id).ToList();

                var entities = lineIds.Count == 0
                    ? new List<WtImportLine>()
                    : await _unitOfWork.WtImportLines.FindAsync(x => x.LineId.HasValue && lineIds.Contains(x.LineId.Value) && !x.IsDeleted);

                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByCellCodeAsync(string cellCode)
        {
            try
            {
                // WtImportLine model doesn't have CellCode property, filtering only by IsDeleted
                var entities = await _unitOfWork.WtImportLines
                    .FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWtImportBarcodeRequestDto request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                // 1) Header kontrolü: İstekle gelen header aktif ve silinmemiş olmalı
                var header = await _unitOfWork.WtHeaders.GetByIdAsync(request.HeaderId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtHeaderNotFound"), _localizationService.GetLocalizedString("WtHeaderNotFound"), 404);
                }

                    // 2) Line uyumluluğu: Aynı header altında stok kodu + yapılandırma kodu ile importLine eşleşme kontrolü
                    var lineControl = await _unitOfWork.WtImportLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);

                    if (lineControl != null && lineControl.Any())
                    {
                        var lineCounter = lineControl.Count(x =>
                            x.StockCode == request.StockCode &&
                            x.YapKod == request.YapKod
                        );

                        if (lineCounter == 0)
                        {
                            return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineStokCodeAndYapCodeNotMatch"), _localizationService.GetLocalizedString("WtImportLineStokCodeAndYapCodeNotMatch"), 404);
                        }
                    }
                  
                    
                    // 3) Seri eşleşme kontrolü: Header'a bağlı LineSerial kayıtları varsa, gelen seriyle eşleşmeli
                    var lineSerialControl = await _unitOfWork.WtLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);

                    if (lineSerialControl != null && lineSerialControl.Any())
                    {
                        var lineSerialCounter = lineSerialControl.Count(x => x.SerialNo == request.SerialNo);
                        if (lineSerialCounter == 0)
                        {
                            return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineSerialNotMatch"), _localizationService.GetLocalizedString("WtImportLineSerialNotMatch"), 404);
                        }
                    }

                    // 4) Mükerrer seri kontrolü: Aynı header + stok + yapkod + seri için daha önce route eklenmiş mi
                    if (!string.IsNullOrWhiteSpace(request.SerialNo))
                    {
                        var duplicateExists = await _unitOfWork.WtRoutes
                                                    .AsQueryable()
                                                    .AnyAsync(r => !r.IsDeleted
                                                    && r.SerialNo == request.SerialNo
                                                    && r.ImportLine.HeaderId == request.HeaderId
                                                    && r.ImportLine.StockCode == request.StockCode
                                                    && r.ImportLine.YapKod == request.YapKod
                                                    );

                        if (duplicateExists)
                        {
                            var msg = _localizationService.GetLocalizedString("WtImportLineDuplicateSerialFound");
                            return ApiResponse<WtImportLineDto>.ErrorResult(msg, msg, 409);
                        }
                    }

                    // 5) Miktar doğrulama: Negatif/0 miktara izin verilmez
                    if (request.Quantity <= 0)
                    {
                        return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineQuantityInvalid"), _localizationService.GetLocalizedString("WtImportLineQuantityInvalid"), 400);
                    }

                    // 6) ImportLine bul/oluştur: Header + Stok + YapKod'a göre mevcut importLine var mı, yoksa yeni oluşturulur
                    WtImportLine? importLine = null;
                    if (request.LineId.HasValue)
                    {
                        importLine = await _unitOfWork.WtImportLines.GetByIdAsync(request.LineId.Value);
                    }
                    else
                    {
                        importLine = (await _unitOfWork.WtImportLines
                            .FindAsync(x => x.HeaderId == request.HeaderId 
                                            && x.StockCode == request.StockCode
                                            && x.YapKod == request.YapKod
                                            && !x.IsDeleted))
                            .FirstOrDefault();
                    }

                    // Kayıt yoksa yeni importLine oluşturulur
                    if (importLine == null)
                    {
                        importLine = new WtImportLine
                        {
                            HeaderId = request.HeaderId,
                            LineId = request.LineId,
                            StockCode = request.StockCode,
                            YapKod = request.YapKod
                        };
                        await _unitOfWork.WtImportLines.AddAsync(importLine);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    // 7) Route kaydı: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                    var route = new WtRoute
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

                    await _unitOfWork.WtRoutes.AddAsync(route);
                    await _unitOfWork.SaveChangesAsync();

                    // 8) Sonuç: importLine DTO döndürülür ve işlem tamamlanır
                    var dto = _mapper.Map<WtImportLineDto>(importLine);

                    scope.Complete();
                    return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<WtImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

    }
}
