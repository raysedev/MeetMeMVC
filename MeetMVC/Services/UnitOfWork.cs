using MeetMVC.Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MeetMVC.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UnitOfWork(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async void UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string fileName = file.FileName.Trim('"');
            fileName = EnsureFileName(fileName);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = File.Create(GetPathAndFileName(fileName)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string EnsureFileName(string fileName)
        {
            if (fileName.Contains("\\"))
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            return fileName;
        }

        private string GetPathAndFileName(string fileName)
        {
            string path = _hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + fileName;
        }

    }
}
