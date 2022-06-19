using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MoviesAPI.CloudServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoviesAPI.LocalServices
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment environment,IHttpContextAccessor httpContextAccessor)
        {
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string route, string container)
        {
            if (route != null)
            {
                string fileName = Path.GetFileName(route);
                string directoryFile = Path.Combine(environment.WebRootPath, container, fileName);

                if (File.Exists(directoryFile))
                {
                    File.Delete(directoryFile);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType)
        {
            await DeleteFile(route, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            string fineName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(environment.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string route = Path.Combine(folder, fineName);
            await File.WriteAllBytesAsync(route, content);

            string actualUri = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            string urlPathDB = Path.Combine(actualUri, container, fineName).Replace("\\", "/");
            return urlPathDB;
        }
    }
}
