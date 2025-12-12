using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PlatformPageGroupService : IPlatformPageGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PlatformPageGroupService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetAllAsync()
        {
            try
            {
                var groups = await _unitOfWork.PlatformPageGroups.GetAllAsync();
                var groupDtos = _mapper.Map<IEnumerable<PlatformPageGroupDto>>(groups);
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.SuccessResult(groupDtos, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PlatformPageGroupDto>> GetByIdAsync(long id)
        {
            try
            {
                var group = await _unitOfWork.PlatformPageGroups.GetByIdAsync(id);
                if (group == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("PlatformPageGroupNotFound");
                    return ApiResponse<PlatformPageGroupDto>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                var groupDto = _mapper.Map<PlatformPageGroupDto>(group);
                return ApiResponse<PlatformPageGroupDto>.SuccessResult(groupDto, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<PlatformPageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PlatformPageGroupDto>> CreateAsync(CreatePlatformPageGroupDto createDto)
        {
            try
            {
                var group = _mapper.Map<PlatformPageGroup>(createDto);
                var createdGroup = await _unitOfWork.PlatformPageGroups.AddAsync(group);
                await _unitOfWork.SaveChangesAsync();
                
                var groupDto = _mapper.Map<PlatformPageGroupDto>(createdGroup);
                return ApiResponse<PlatformPageGroupDto>.SuccessResult(groupDto, _localizationService.GetLocalizedString("PlatformPageGroupCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupCreationError");
                return ApiResponse<PlatformPageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PlatformPageGroupDto>> UpdateAsync(long id, UpdatePlatformPageGroupDto updateDto)
        {
            try
            {
                var existingGroup = await _unitOfWork.PlatformPageGroups.GetByIdAsync(id);
                if (existingGroup == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("PlatformPageGroupNotFound");
                    return ApiResponse<PlatformPageGroupDto>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                if (updateDto.GroupName != null)
                    existingGroup.GroupName = updateDto.GroupName;

                if (updateDto.GroupCode != null)
                    existingGroup.GroupCode = updateDto.GroupCode;

                if (updateDto.MenuHeaderId.HasValue)
                    existingGroup.MenuHeaderId = updateDto.MenuHeaderId;

                if (updateDto.MenuLineId.HasValue)
                    existingGroup.MenuLineId = updateDto.MenuLineId;

                _unitOfWork.PlatformPageGroups.Update(existingGroup);
                await _unitOfWork.SaveChangesAsync();

                var groupDto = _mapper.Map<PlatformPageGroupDto>(existingGroup);
                return ApiResponse<PlatformPageGroupDto>.SuccessResult(groupDto, _localizationService.GetLocalizedString("PlatformPageGroupUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupUpdateError");
                return ApiResponse<PlatformPageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var group = await _unitOfWork.PlatformPageGroups.GetByIdAsync(id);
                if (group == null)
                {
                    var notFoundMessage = _localizationService.GetLocalizedString("PlatformPageGroupNotFound");
                    return ApiResponse<bool>.ErrorResult(notFoundMessage, notFoundMessage, 404);
                }

                await _unitOfWork.PlatformPageGroups.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PlatformPageGroupDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupDeletionError");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetByGroupCodeAsync(string groupCode)
        {
            try
            {
                var groups = await _unitOfWork.PlatformPageGroups.FindAsync(g => g.GroupCode == groupCode);
                var groupDtos = _mapper.Map<IEnumerable<PlatformPageGroupDto>>(groups);
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.SuccessResult(groupDtos, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetByMenuHeaderIdAsync(int menuHeaderId)
        {
            try
            {
                var groups = await _unitOfWork.PlatformPageGroups.FindAsync(g => g.MenuHeaderId == menuHeaderId);
                var groupDtos = _mapper.Map<IEnumerable<PlatformPageGroupDto>>(groups);
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.SuccessResult(groupDtos, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetByMenuLineIdAsync(int menuLineId)
        {
            try
            {
                var groups = await _unitOfWork.PlatformPageGroups.FindAsync(g => g.MenuLineId == menuLineId);
                var groupDtos = _mapper.Map<IEnumerable<PlatformPageGroupDto>>(groups);
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.SuccessResult(groupDtos, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetPageGroupsGroupByGroupCodeAsync()
        {
            try
            {
                var groupedByGroupCode = await _unitOfWork.PlatformPageGroups.AsQueryable()
                    .GroupBy(pg => pg.GroupCode)
                    .Select(g => g.First())
                    .ToListAsync();

                var groupDtos = _mapper.Map<IEnumerable<PlatformPageGroupDto>>(groupedByGroupCode);
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.SuccessResult(groupDtos, _localizationService.GetLocalizedString("PlatformPageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("PlatformPageGroupRetrievalError");
                return ApiResponse<IEnumerable<PlatformPageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }
    }
}
