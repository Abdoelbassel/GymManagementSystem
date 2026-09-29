using GymManagmentSystem.BLL.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Attachment
{
    public interface IAttachmentServices
    {
        public Task<Result<string>> UploadAsync(Stream fileStream , string fileName , string FolderName , CancellationToken ct = default);

        public Result DeleteAsync(string fileName, string folderName, CancellationToken ct = default);

        public Result<(Stream stream, string contentType)> GetFile(string fileName, string folderName);
    }
}
