using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TNShopSolution.Application.Common
{
    public class StorageService : IStorageService
    {
        private readonly string useContentFoder;
        private const string USER_CONTENT_FODER_NAME = "user-content";

        public StorageService(IWebHostEnvironment webHostEnviroment)
        {
            useContentFoder = Path.Combine(webHostEnviroment.ContentRootPath, USER_CONTENT_FODER_NAME);
        }
        public async Task DeleteFile(string fileName)
        {
            var file = Path.Combine(useContentFoder,fileName);
            if(File.Exists(fileName))
            {
                await Task.Run(() => File.Delete(fileName));
            }
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FODER_NAME }/{fileName}";
        }

        public async Task SaveFile(Stream mediaBinayStream, string fileName)
        {
            var file = Path.Combine(useContentFoder, fileName);
            using var output = new FileStream(file, FileMode.Create);
            await mediaBinayStream.CopyToAsync(output);
        }
    }
}
