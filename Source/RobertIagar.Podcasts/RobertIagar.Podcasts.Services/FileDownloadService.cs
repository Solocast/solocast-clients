using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Services.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace RobertIagar.Podcasts.Services
{
    public class FileDownloadService : IFileDownloadService
    {
        private Dictionary<string, DownloadOperation> downloads;
        private Dictionary<string, CancellationTokenSource> cancellationTokenSources;

        public FileDownloadService()
        {
            downloads = new Dictionary<string, DownloadOperation>();
            cancellationTokenSources = new Dictionary<string, CancellationTokenSource>();
        }

        public int NumberOfDownlaods
        {
            get
            {
                return downloads.Count;
            }
        }

        public void CancelAllDownloads()
        {
            foreach (var cancellationTokenSource in cancellationTokenSources.ToList())
            {
                CancelDownload(cancellationTokenSource.Key);
            }
        }

        public void CancelDownload(string fileUrl)
        {
            var cancellationTokenSource = cancellationTokenSources[fileUrl];
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSources.Remove(fileUrl);
            downloads.Remove(fileUrl);
        }

        public async Task<StorageFile> DownloadFileAsync(
            string appFolderName,
            string folderName,
            string fileName,
            string fileUrl,
            Action<DownloadOperation> callback,
            Action<Exception> errorCallback = null)
        {
            var appFolder = await KnownFolders.MusicLibrary.CreateFolderAsync(appFolderName, CreationCollisionOption.OpenIfExists);
            var podcastFolder = await appFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            try
            {
                var uri = new Uri(fileUrl);
                var extension = uri.AbsolutePath.GetExtension();
                var file = await podcastFolder.CreateFileAsync(fileName + extension, CreationCollisionOption.ReplaceExisting);
                var backgroundDownloader = new BackgroundDownloader();
                var downloadOperation = backgroundDownloader.CreateDownload(uri, file);
                var progress = new Progress<DownloadOperation>(callback);
                downloads.Add(fileUrl, downloadOperation);

                var cts = new CancellationTokenSource();
                cancellationTokenSources.Add(fileUrl, cts);

                await downloadOperation.StartAsync().AsTask(cts.Token, progress);

                downloads.Remove(fileUrl);
                cancellationTokenSources.Remove(fileUrl);
                return file;
            }
            catch (Exception ex)
            {
                if (errorCallback != null)
                    errorCallback(ex);
                return null;
            }
        }

        public void PauseDownload(string fileUrl)
        {
            var downloadOperation = downloads[fileUrl];
            if (downloadOperation.Progress.Status == BackgroundTransferStatus.Running)
            {
                downloadOperation.Pause();
            }
        }

        public void ResumeDownload(string fielUrl)
        {
            var downloadOperation = downloads[fielUrl];
            if (downloadOperation.Progress.Status == BackgroundTransferStatus.PausedByApplication)
            {
                downloadOperation.Resume();
            }
        }
    }
}
