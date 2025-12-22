using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PrImportLineService : IPrImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public PrImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrImportLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                var dto = _mapper.Map<PrImportLineDto>(entity);
                var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
                if (!enrichedSingle.Success)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
                }
                var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
                return ApiResponse<PrImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.PrImportLines.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PrImportLineDto>>(entities);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<PrImportLineDto>> CreateAsync(CreatePrImportLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PrImportLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.PrImportLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrImportLineDto>(entity);
                return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrImportLineDto>> UpdateAsync(long id, UpdatePrImportLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.PrImportLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PrImportLineDto>(entity);
                return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PrImportLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineNotFound"), _localizationService.GetLocalizedString("PrImportLineNotFound"), 404);
                }
                var routes = await _unitOfWork.PrRoutes.FindAsync(x => x.ImportLineId == id && !x.IsDeleted);
                if (routes.Any())
                {
                    var msg = _localizationService.GetLocalizedString("PrImportLineRoutesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                var hasActiveLineSerials = await _unitOfWork.PrLineSerials
                    .AsQueryable()
                    .AnyAsync(ls => !ls.IsDeleted && ls.LineId == entity.LineId);
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("PrImportLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.PrImportLines.SoftDelete(id);

                    var headerId = entity.HeaderId;
                    var hasOtherLines = await _unitOfWork.PrLines
                        .AsQueryable()
                        .AnyAsync(l => !l.IsDeleted && l.HeaderId == headerId);
                    var hasOtherImportLines = await _unitOfWork.PrImportLines
                        .AsQueryable()
                        .AnyAsync(il => !il.IsDeleted && il.HeaderId == headerId);
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.PrHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrImportLineDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
            try
            {
                var header = await _unitOfWork.PrHeaders.GetByIdAsync(headerId);
                if (header == null || header.IsDeleted)
                {
                    return ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderNotFound"), _localizationService.GetLocalizedString("PrHeaderNotFound"), 404);
                }

                var importLines = await _unitOfWork.PrImportLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var items = new List<PrImportLineWithRoutesDto>();

                foreach (var il in importLines)
                {
                    var routes = await _unitOfWork.PrRoutes.FindAsync(r => r.ImportLineId == il.Id && !r.IsDeleted);
                    var dto = new PrImportLineWithRoutesDto
                    {
                        ImportLine = _mapper.Map<PrImportLineDto>(il),
                        Routes = _mapper.Map<List<PrRouteDto>>(routes)
                    };
                    items.Add(dto);
                }

                var importLineDtos = items.Select(i => i.ImportLine).ToList();
                var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>.ErrorResult(
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

                return ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddPrImportBarcodeRequestDto request)
        {
            try
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        // 1) Header kontrolü: İstekle gelen header aktif ve silinmemiş olmalı
                        var header = await _unitOfWork.PrHeaders.GetByIdAsync(request.HeaderId);
                        if (header == null || header.IsDeleted)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderNotFound"), _localizationService.GetLocalizedString("PrHeaderNotFound"), 404);
                        }

                        // 2) Line uyumluluğu: Aynı header altında stok kodu + yapılandırma kodu ile importLine eşleşme kontrolü
                        var lineControl = await _unitOfWork.PrLines.FindAsync(x => x.HeaderId == request.HeaderId && !x.IsDeleted);

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
                                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineStokCodeAndYapCodeNotMatch"), _localizationService.GetLocalizedString("PrImportLineStokCodeAndYapCodeNotMatch"), 404);
                            }
                        }

                        // 3) Seri eşleşme kontrolü: Header'a bağlı LineSerial kayıtları varsa, gelen seriyle eşleşmeli
                        var lineSerialControl = await _unitOfWork.PrLineSerials.FindAsync(x => !x.IsDeleted && x.Line.HeaderId == request.HeaderId);
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
                                    return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineSerialNotMatch"), _localizationService.GetLocalizedString("PrImportLineSerialNotMatch"), 404);
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
                                var duplicateExists = await _unitOfWork.PrRoutes
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
                                    var msg = _localizationService.GetLocalizedString("PrImportLineDuplicateSerialFound");
                                    return ApiResponse<PrImportLineDto>.ErrorResult(msg, msg, 409);
                                }
                            }
                        }

                        // 5) Miktar doğrulama: Negatif/0 miktara izin verilmez
                        if (request.Quantity <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineQuantityInvalid"), _localizationService.GetLocalizedString("PrImportLineQuantityInvalid"), 400);
                        }

                        // 6) ImportLine bul/oluştur: Header + Stok + YapKod'a göre mevcut importLine var mı, yoksa yeni oluşturulur
                        PrImportLine? importLine = null;
                        if (request.LineId.HasValue)
                        {
                            importLine = await _unitOfWork.PrImportLines.GetByIdAsync(request.LineId.Value);
                        }
                        else
                        {
                            importLine = (await _unitOfWork.PrImportLines
                                .FindAsync(x => x.HeaderId == request.HeaderId 
                                                && ((x.StockCode ?? "").Trim() == (request.StockCode ?? "").Trim())
                                                && ((x.YapKod ?? "").Trim() == (request.YapKod ?? "").Trim())
                                                && !x.IsDeleted))
                                .FirstOrDefault();
                        }

                        // Kayıt yoksa yeni importLine oluşturulur
                        if (importLine == null)
                        {
                            var createImportLineDto = new CreatePrImportLineDto
                            {
                                HeaderId = request.HeaderId,
                                LineId = request.LineId,
                                StockCode = request.StockCode,
                                YapKod = request.YapKod
                            };
                            importLine = _mapper.Map<PrImportLine>(createImportLineDto);
                            await _unitOfWork.PrImportLines.AddAsync(importLine);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // 7) Route kaydı: Barkod, miktar, seri ve lokasyon bilgileri ile importLine'a bağlı route eklenir
                        var createRouteDto = new CreatePrRouteDto
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
                        var route = _mapper.Map<PrRoute>(createRouteDto);

                        await _unitOfWork.PrRoutes.AddAsync(route);
                        await _unitOfWork.SaveChangesAsync();

                        // 8) Sonuç: importLine DTO döndürülür ve işlem tamamlanır
                        await _unitOfWork.CommitTransactionAsync();
                        var dto = _mapper.Map<PrImportLineDto>(importLine);
                        return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineCreatedSuccessfully"));
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
                return ApiResponse<PrImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("PrImportLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
