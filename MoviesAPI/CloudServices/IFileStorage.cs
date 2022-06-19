using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.CloudServices
{
    public interface IFileStorage
    {
        Task<string> SaveFile(byte[] content, string extension, string container, string contentType);
        Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType);
        Task DeleteFile(string route, string container);
    }
}
