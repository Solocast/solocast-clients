using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;
using RobertIagar.Podcasts.Core.Contracts;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Popups;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class EpisodeViewModel : ViewModelBase, IProgressReport
    {
        private Episode episode;
        private IFileDownloadService fileDownloadService;
        private IMessageDialogService messageDialogService;
        private float percent;

        public EpisodeViewModel(Episode episode)
        {
            this.fileDownloadService = ServiceLocator.Current.GetInstance<IFileDownloadService>();
            this.messageDialogService = ServiceLocator.Current.GetInstance<IMessageDialogService>();
            this.DownloadCommand = new RelayCommand(async () => await DownloadEpisodeAsync(), CanDownloadEpisode);

            this.PlayCommand = new RelayCommand(PlayEpisode, CanPlayEpisode);
            this.Episode = episode;
        }

        public Episode Episode
        {
            get { return episode; }
            set { Set(nameof(Episode), ref episode, value); }
        }

        public bool IsLocal
        {
            get { return episode.IsLocal; }
        }

        public float Percent
        {
            get
            {
                if (episode.IsLocal)
                    return 100;
                return percent;
            }
            set { Set(nameof(Percent), ref percent, value); }
        }

        public ICommand DownloadCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand DeleteCommand { get; }

        private async Task DownloadEpisodeAsync()
        {
            var file = await fileDownloadService.DownloadFileAsync(
                "Solocast",
                episode.Podcast.Title,
                $"{episode.Title}",
                episode.Path,
                ReportProgress,
                ErrorCallback);

            if (file != null)
            {
                Episode.Path = file.Path;
                RaisePropertyChanged(nameof(IsLocal));
                (DownloadCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanDownloadEpisode()
        {
            if (Episode.IsLocal)
                return false;

            return true;
        }

        private void PlayEpisode()
        {
            MessengerInstance.Send(new PlayEpisodeMessage(episode));
        }

        private bool CanPlayEpisode()
        {
            return true;
        }

        private void DeleteEpisode()
        {
            
        }

        private bool CanDeleteEpisode()
        {
            return false;
        }

        public async void ReportProgress(DownloadOperation operation)
        {
            var received = operation.Progress.BytesReceived;
            var totalToReceive = operation.Progress.TotalBytesToReceive;

            var percent = (received * 100) / totalToReceive;

            await DispatcherHelper.RunAsync(() =>
            {
                Percent = percent;
            });
        }

        public void ErrorCallback(Exception exception)
        {
            messageDialogService.ShowDialogAsync(exception.Message, "Error!", new[]
            {
                new UICommand("OK",null, 1)
            }, 1);
        }
    }
}
