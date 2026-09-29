using GymManagmentSystem.BLL.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Attachment
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly long maxFilSize = 5 * 1024 * 1024;
        private readonly ILogger<IAttachmentServices> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string[] allowedExtensions = { ".jpg" , ".jpeg" , ".png"};
        public AttachmentServices(ILogger<IAttachmentServices> logger , IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public Result DeleteAsync(string fileName, string folderName, CancellationToken ct = default)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);

            try
            {
                if (!File.Exists(fullPath))
                    return Result.NotFound("File Attachment Not Found");

                File.Delete(fullPath);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed To Delete Attachment");
                return Result.Fail("Failed To Delete Attachment");
            }
        }

        public Result<(Stream stream, string contentType)> GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName))
                return Result<(Stream, string)>.Fail("File Name Or Folder Name Is Invalid ");

            string fullpath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(fullpath))
                return Result<(Stream, string)>.Fail("Path is Not Exists");

            var stream = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(fileName).ToLower();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream" // Binary Data
            };

            return Result<(Stream, string)>.Ok((stream, contentType));
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string FolderName, CancellationToken ct = default)
        {
            if (fileStream == null || !fileStream.CanRead || fileStream.Length == 0)
                return Result<string>.ValidationError("Invalid File");


            if (fileStream.Length > maxFilSize)
            {
                _logger.LogError($"Rejected File : File Too Large {fileName} Bytes");
                return Result<string>.ValidationError("Size must be less than 5 MB");

            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) ||
                !allowedExtensions.Contains(extension))
            {
                _logger.LogError($"Rejected File : File {fileName} Not Allowed");
                return Result<string>.ValidationError("File Not Allowed");
            }

            var uploadFolder = Path.Combine(_env.ContentRootPath, FolderName);
            Directory.CreateDirectory(uploadFolder);

            var storedFileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(uploadFolder, storedFileName);

            try
            {
                using var st = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(st , ct);
                return Result<string>.Ok(storedFileName);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed To Uploead File ");
                return Result<string>.Fail("Failed To Uploaded");
            }


        }
    }
}
