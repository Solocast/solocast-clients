using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RobertIagar.Podcasts.Core.Services
{
    public interface IFileDownloader
    {
        Task<StorageFile> DonwloadFileAsync(string fileUrl);
        Task<StorageFile> DownloadFileAsync(Uri fileUrl);
        Task SaveFileAsync(string filePath);
    }
}
