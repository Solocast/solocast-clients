using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Solocast.Core.Interfaces
{
    public interface IFileDownloadService
    {
        Task<StorageFile> DownloadFileAsync(
            string appFolderName,
            string folderName,
            string fileName,
            string fileUrl,
            Action<DownloadOperation> callback,
            Action<Exception> errorCallback = null);
        void PauseDownload(string fileUrl);
        void ResumeDownload(string fielUrl);
        void CancelDownload(string fileUrl);
        void CancelAllDownloads();
        int NumberOfDownlaods { get; }
    }
}
