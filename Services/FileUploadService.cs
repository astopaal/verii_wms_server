using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace WMS_WEBAPI.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILocalizationService _localizationService;
        private const string ProfilePicturesFolder = "uploads/user-profiles";
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public FileUploadService(IWebHostEnvironment environment, ILocalizationService localizationService)
        {
            _environment = environment;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<string>> UploadProfilePictureAsync(IFormFile file, long userId)
        {
            try
            {
                // Validation
                if (file == null || file.Length == 0)
                {
                    return ApiResponse<string>.ErrorResult(
                        _localizationService.GetLocalizedString("FileRequired"),
                        "File is required",
                        400);
                }

                if (file.Length > MaxFileSize)
                {
                    return ApiResponse<string>.ErrorResult(
                        _localizationService.GetLocalizedString("FileSizeExceeded"),
                        $"File size exceeds {MaxFileSize / (1024 * 1024)} MB",
                        400);
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return ApiResponse<string>.ErrorResult(
                        _localizationService.GetLocalizedString("InvalidFileFormat"),
                        $"Allowed formats: {string.Join(", ", AllowedExtensions)}",
                        400);
                }

                // Create directory in project root/uploads (not wwwroot)
                var uploadsPath = Path.Combine(_environment.ContentRootPath, ProfilePicturesFolder);
                var userFolder = Path.Combine(uploadsPath, userId.ToString());
                
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    SetDirectoryPermissions(uploadsPath);
                }
                
                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                    SetDirectoryPermissions(userFolder);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(userFolder, fileName);

                // Save file using proper async/await with resource disposal
                await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await using (var sourceStream = file.OpenReadStream())
                    {
                        await sourceStream.CopyToAsync(fileStream).ConfigureAwait(false);
                        await fileStream.FlushAsync().ConfigureAwait(false);
                    }
                }

                // Return URL
                var url = GetProfilePictureUrl(fileName, userId);
                return ApiResponse<string>.SuccessResult(url, _localizationService.GetLocalizedString("FileUploadedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(
                    _localizationService.GetLocalizedString("FileUploadError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteProfilePictureAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NoFileToDelete"));
                }

                // Extract file path from URL
                // URL format: /uploads/user-profiles/{userId}/{fileName}
                // Handle both relative and absolute URLs
                string pathToParse = fileUrl.Trim();
                
                // Remove query string if exists
                if (pathToParse.Contains('?'))
                {
                    pathToParse = pathToParse.Substring(0, pathToParse.IndexOf('?'));
                }

                // Handle absolute URLs
                if (Uri.TryCreate(pathToParse, UriKind.Absolute, out Uri? absoluteUri))
                {
                    pathToParse = absoluteUri.AbsolutePath;
                }
                
                // Ensure path starts with /
                if (!pathToParse.StartsWith("/"))
                {
                    pathToParse = "/" + pathToParse;
                }

                var pathSegments = pathToParse.Split('/', StringSplitOptions.RemoveEmptyEntries);
                
                if (pathSegments.Length < 4 || pathSegments[0] != "uploads" || pathSegments[1] != "user-profiles")
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("InvalidFileUrl"),
                        $"Invalid file URL format. Expected: /uploads/user-profiles/{{userId}}/{{fileName}}, Got: {fileUrl}",
                        400);
                }

                var userId = pathSegments[2];
                var fileName = pathSegments[3];
                
                // Build file path using ContentRootPath (not WebRootPath)
                var filePath = Path.Combine(
                    _environment.ContentRootPath,
                    "uploads",
                    "user-profiles",
                    userId,
                    fileName);

                if (File.Exists(filePath))
                {
                    // Delete file synchronously (File.Delete is already synchronous and fast)
                    File.Delete(filePath);
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("FileDeletedSuccessfully"));
                }
                else
                {
                    // File doesn't exist, but that's okay - it might have been already deleted
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("FileDeletedSuccessfully"));
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("FileDeletionError"),
                    ex.Message,
                    500);
            }
        }

        public string GetProfilePictureUrl(string fileName, long userId)
        {
            // Return relative URL that will be served by static files middleware
            return $"/{ProfilePicturesFolder}/{userId}/{fileName}";
        }

        /// <summary>
        /// Sets read/write permissions for the directory (Windows only)
        /// </summary>
        private void SetDirectoryPermissions(string directoryPath)
        {
            // Only set permissions on Windows
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            try
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                var directorySecurity = directoryInfo.GetAccessControl();

                // Get current user
                var currentUser = WindowsIdentity.GetCurrent();
                if (currentUser != null && currentUser.User != null)
                {
                    // Add full control for current user
                    var accessRule = new FileSystemAccessRule(
                        currentUser.User,
                        FileSystemRights.FullControl,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow);

                    directorySecurity.AddAccessRule(accessRule);
                    directoryInfo.SetAccessControl(directorySecurity);
                }

                // Also add permissions for IIS_IUSRS (if running under IIS)
                try
                {
                    var iisUser = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                    var iisAccessRule = new FileSystemAccessRule(
                        iisUser,
                        FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow);

                    directorySecurity.AddAccessRule(iisAccessRule);
                    directoryInfo.SetAccessControl(directorySecurity);
                }
                catch
                {
                    // Ignore if IIS_IUSRS doesn't exist (e.g., running outside IIS)
                }

                // Add permissions for Everyone (for development/testing)
                // In production, you might want to remove this or make it more restrictive
                try
                {
                    var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                    var everyoneAccessRule = new FileSystemAccessRule(
                        everyone,
                        FileSystemRights.ReadAndExecute | FileSystemRights.Write,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow);

                    directorySecurity.AddAccessRule(everyoneAccessRule);
                    directoryInfo.SetAccessControl(directorySecurity);
                }
                catch
                {
                    // Ignore if setting permissions fails
                }
            }
            catch (Exception)
            {
                // Silently fail if permission setting is not supported (e.g., on Linux)
                // The directory will still be created, but permissions might need to be set manually
            }
        }
    }
}
