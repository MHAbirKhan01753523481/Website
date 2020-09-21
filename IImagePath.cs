using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.IServices
{
    public interface IImagePath
    {
        string GetImagePath(string fileName, string parentFolderName, string folderName);
        string GetImagePath(string fileName, string parentFolderName);
        string GetImagePathForDb(string imagePath);
        void RemoveFileOfPath(string path);
        string GetSourceUrlToFilePath(string sourceUrl);
        Task<string> SaveToFolderAndReturnPathForEmployee(IFormFile file, string employeeId);
        string GetFilePathAsSourceUrl(string directoryPath);
        byte[] GetImageFromUrl(string path);
    }
}
