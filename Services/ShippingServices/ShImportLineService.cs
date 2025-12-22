using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShImportLineService : IShImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public ShImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<ShImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<ShImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<ShImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.ShImportLines.FindAsync(x => x.LineId == lineId);
                var dtos = _mapper.Map<IEnumerable<ShImportLineDto>>(entities);

                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShImportLine>(createDto);
                var created = await _unitOfWork.ShImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(created);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (existing == null)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<ShImportLineDto>.ErrorResult(nf, nf, 404);
                }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShImportLineDto>(entity);
                return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShImportLineNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var routes = await _unitOfWork.ShRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("ShImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var hasActiveLineSerials = await _unitOfWork.ShLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("ShImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.ShImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.ShLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.ShImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.ShHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.ShHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderNotFound"), _localizationService.GetLocalizedString("ShHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.ShImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var items = new List<ShImportLineWithRoutesDto>();

                foreach (var il in importLines)
                {
                    var routes = await _unitOfWork.ShRoutes.FindAsync(r => r.ImportLineId == il.Id && !r.IsDeleted);
                    var dto = new ShImportLineWithRoutesDto
                    {
                        ImportLine = _mapper.Map<ShImportLineDto>(il),
                        Routes = _mapper.Map<List<ShRouteDto>>(routes)
                    };
                    items.Add(dto);
                }

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(
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

                return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddShImportBarcodeRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        // 1) Header kontrolü: İstekle gelen header aktif ve silinmemiş olmalı
                        var header = await _unitOfWork.ShHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShHeaderNotFound"), _localizationService.GetLocalizedString("ShHeaderNotFound"), 404);
                        }

                        // 2) Line uyumluluğu: Aynı header altında stok kodu + yapılandırma kodu ile importLine eşleşme kontrolü
                        var lineControl = await _unitOfWork.ShLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);

                        if (lineControl != null && lineControl.Any())
                        {
                            var reqStock = (request.StockCode ?? "").Trim();
                            var reqYap = (request.YapKod ?? "").Trim();
                            var lineCounter = lineControl.Count(x =>
                                ((x.StockCode ?? "").Trim() == reqStock) && ((x.YapKod ?? "").Trim() == reqYap)
                            );
                            if (lineCounter == 0)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineStokCodeAndYapCodeNotMatch"), _localizationService.GetLocalizedString("ShImportLineStokCodeAndYapCodeNotMatch"), 404);
                            }
                        }

                        // 3) Seri eşleşme kontrolü: Header'a bağlı LineSerial kayıtları varsa, gelen seriyle eşleşmeli
                        var lineSerialControl = await _unitOfWork.ShLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
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
                                    await _unitOfWork.RollbackTransactionAsync();
                                    return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineSerialNotMatch"), _localizationService.GetLocalizedString("ShImportLineSerialNotMatch"), 404);
                                }
                            }
                        }

                        // 4) Mükerrer seri kontrolü: Aynı header + stok + yapkod + seri için daha önce route eklenmiş mi
                        {
                            var s1 = (request.SerialNo ?? "").Trim();
                            var s2 = (request.SerialNo2 ?? "").Trim();
                            var s3 = (request.SerialNo3 ?? "").Trim();
                            var s4 = (request.SerialNo4 ?? "").Trim();
                            var anyReqSerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                            if (anyReqSerial)
                            {
                                var duplicateExists = await _unitOfWork.ShRoutes
                                                                    .AsQueryable()
                                                                    .AnyAsync(r => !r.IsDeleted
                                                                    && r.ImportLine.HeaderId == request.HeaderId
                                                                    && ((r.ImportLine.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim())
                                                                    && ((r.ImportLine.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim())
                                                                    && (
                                                                        (!string.IsNullOrWhiteSpace(s1) && ((r.SerialNo ?? "").Trim() == s1)) ||
                                                                        (!string.IsNullOrWhiteSpace(s2) && ((r.SerialNo2 ?? "").Trim() == s2)) ||
                                                                        (!string.IsNullOrWhiteSpace(s3) && ((r.SerialNo3 ?? "").Trim() == s3)) ||
                                                                        (!string.IsNullOrWhiteSpace(s4) && ((r.SerialNo4 ?? "").Trim() == s4))
                                                                    )
                                                                    );
                                if (duplicateExists)
                                {
                                    await _unitOfWork.RollbackTransactionAsync();
                                    var msg = _localizationService.GetLocalizedString("ShImportLineDuplicateSerialFound");
                                    return ApiResponse<ShImportLineDto>.ErrorResult(msg, msg, 409);
                                }
                            }
                        }

                        // 5) Miktar doğrulama: Negatif/0 miktara izin verilmez
                        if (request.Quantity <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineQuantityInvalid"), _localizationService.GetLocalizedString("ShImportLineQuantityInvalid"), 400);
                        }

                        // 6) ImportLine bul/oluştur: Header + Stok + YapKod'a göre mevcut importLine var mı, yoksa yeni oluşturulur
                        ShImportLine? importLine = null;
                        if (request.LineId.HasValue)
                        {
                            importLine = await _unitOfWork.ShImportLines.GetByIdAsync(request.LineId.Value);
                        }
                        else
                        {
                            importLine = (await _unitOfWork.ShImportLines
                                .FindAsync(x => x.HeaderId == request.HeaderId 
                                                && ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim())
                                                && ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim())
                                                && !x.IsDeleted))
                                .FirstOrDefault();
                        }

                        // Kayıt yoksa yeni importLine oluşturulur
                        if (importLine == null)
                        {
                            var createImportLineDto = new CreateShImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = request.LineId,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<ShImportLine>(createImportLineDto);
                            await _unitOfWork.ShImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // 7) Route kaydı: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        var createRouteDto = new CreateShRouteDto
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
                        var route = _mapper.Map<ShRoute>(createRouteDto);

                        await _unitOfWork.ShRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // 8) Sonuç: importLine DTO döndürülür ve işlem tamamlanır
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<ShImportLineDto>(importLine);
                        return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
                    }
                    catch
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<ShImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
