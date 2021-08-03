using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.Application.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);
        Task SaveFile(Stream mediaBinayStream, string fileName);
        Task DeleteFile(string fileName);
    }
}
