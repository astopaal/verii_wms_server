using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrImportDocumentService : IGrImportDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public GrImportDocumentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<GrImportDocumentDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.GrImportDocuments.AsQueryable();
                query = query.ApplyFilters(request.Filters);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<GrImportDocumentDto>>(items);

                var result = new PagedResponse<GrImportDocumentDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<GrImportDocumentDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrImportDocumentDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetAllAsync()
        {
            try
            {
                var documents = await _unitOfWork.GrImportDocuments.GetAllAsync();
                var documentDtos = _mapper.Map<IEnumerable<GrImportDocumentDto>>(documents);

                return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(documentDtos, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportDocumentDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportDocumentDto>> GetByIdAsync(long id)
        {
            try
            {
                var document = await _unitOfWork.GrImportDocuments.GetByIdAsync(id);
                if (document == null)
                {
                    return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentNotFound"), "Record not found", 404, "GrImportDocument not found");
                }

                var documentDto = _mapper.Map<GrImportDocumentDto>(document);
                return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentGetByIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var documents = await _unitOfWork.GrImportDocuments.FindAsync(d => d.HeaderId == headerId);
                var documentDtos = _mapper.Map<IEnumerable<GrImportDocumentDto>>(documents);

                return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(documentDtos, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrImportDocumentDto>>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentGetByHeaderIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportDocumentDto>> CreateAsync(CreateGrImportDocumentDto createDto)
        {
            try
            {
                // HeaderId'nin geçerli olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)createDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentInvalidHeaderId"), "Invalid header ID", 404, "Header not found");
                }

                var document = _mapper.Map<GrImportDocument>(createDto);
                var createdDocument = await _unitOfWork.GrImportDocuments.AddAsync(document);
                await _unitOfWork.SaveChangesAsync();

                var documentDto = _mapper.Map<GrImportDocumentDto>(createdDocument);
                return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentCreateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrImportDocumentDto>> UpdateAsync(long id, UpdateGrImportDocumentDto updateDto)
        {
            try
            {
                var document = await _unitOfWork.GrImportDocuments.GetByIdAsync(id);
                if (document == null)
                {
                    return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentNotFound"), "Record not found", 404, "GrImportDocument not found");
                }

                // HeaderId'nin geçerli olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)updateDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentInvalidHeaderId"), "Invalid header ID", 404, "Header not found");
                }

                _mapper.Map(updateDto, document);
                _unitOfWork.GrImportDocuments.Update(document);
                await _unitOfWork.SaveChangesAsync();

                var documentDto = _mapper.Map<GrImportDocumentDto>(document);
                return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var document = await _unitOfWork.GrImportDocuments.GetByIdAsync(id);
                if (document == null)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentNotFound"), "Record not found", 404, "GrImportDocument not found");
                }

                await _unitOfWork.GrImportDocuments.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportDocumentDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrImportDocumentDeleteError"), ex.Message, 500);
            }
        }

        
    }
}
