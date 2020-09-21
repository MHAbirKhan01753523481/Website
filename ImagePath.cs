using Alphasoft.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Services
{
    public class ImagePath : IImagePath
    {
        private readonly IHostingEnvironment _environment;
        public ImagePath(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public string GetImagePath(string fileName, string parentFolderName, string folderName)
        {
            string path = _environment.WebRootPath + "\\" + "images" + "\\" + parentFolderName + "\\" + folderName + "\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }

        public async Task<string> SaveToFolderAndReturnPathForEmployee(IFormFile file, string employeeId)
        {
            var uploadPath = _environment.WebRootPath + "\\" + "images" + "\\" + "uploads" + "\\" + "Employee" + "\\";

            var filePath = "";

            if (file.Length > 0)
            {
                var extension = string.IsNullOrEmpty(Path.GetExtension(file.FileName)) ? ".jpg" : Path.GetExtension(file.FileName);

                filePath = Path.Combine(uploadPath, employeeId + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            var relativePath = GetAsApplicationWebrootPath(filePath);

            return relativePath;
        }
        private string GetAsApplicationWebrootPath(string path)
        {
            var contentRootPath = _environment.ContentRootPath;

            var relativePath = path.Replace(contentRootPath, "");

            return relativePath;
        }



        public string GetImagePath(string fileName, string parentFolderName)
        {
            string path = _environment.WebRootPath + "\\" + "images" + "\\" + parentFolderName + "\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }

        public string GetImagePathForDb(string imagePath)
        {
            string webRootFolder = _environment.WebRootPath;

            imagePath = imagePath.Replace(webRootFolder, "");

            imagePath = imagePath.Replace(@"\", "/");

            return imagePath;
        }

        public void RemoveFileOfPath(string path)
        {
            if (path == null)
            {
                path = "";
            }

            var uploadedPath = _environment.ContentRootPath + @"\wwwroot" + path;

            if (File.Exists(uploadedPath))
            {
                // If file found, delete it    
                File.Delete(uploadedPath);
            }
        }

        public string GetSourceUrlToFilePath(string path)
        {
            var sourceUrl = path.Replace("/", @"\");

            return sourceUrl;
        }
        public string GetFilePathAsSourceUrl(string directoryPath)
        {
            var sourceUrl = directoryPath.Replace(@"\wwwroot", "");

            sourceUrl = sourceUrl.Replace(@"\", "/");

            return sourceUrl;
        }
        public byte[] GetImageFromUrl(string path)
        {
            path = path.Replace(@"\wwwroot", "");
            var image = System.IO.File.ReadAllBytes(_environment.WebRootFileProvider.GetFileInfo(path).PhysicalPath);
            return image;
        }
    }
}
