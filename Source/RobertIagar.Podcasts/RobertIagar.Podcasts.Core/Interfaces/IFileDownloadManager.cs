using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface IFileDownloadManager
    {
        Task<StorageFile> DonwloadFileAsync(
            string appFolderName,
            string folderName,
            string fileName,
            string fileUrl,
            Action<DownloadOperation> callback,
            Action errorCallback = null);
        void PauseDownload(string fileUrl);
        void ResumeDownload(string fielUrl);
        void CancelDownload(string fileUrl);
        void CancelAllDownloads();
        int NumberOfDownlaods { get; }
    }
}
