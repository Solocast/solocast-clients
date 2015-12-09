using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Services
{
    public interface IPlayService
    {
        Task PlayEpisodeAsync(Episode episode);
        Task PauseAsync();
        Task StopAsync();
        Task ResumeAsync();
        void SetupBackgroundAudio();
        void UpdateSystemControls(Episode episode);
    }

    public class PlayService : IPlayService
    {
        private MediaElement player;
        private SystemMediaTransportControls systemControls;

        public PlayService(SystemMediaTransportControls systemControls, MediaElement player)
        {
            this.systemControls = systemControls;
            this.player = player;
            Messenger.Default.Register<PlayEpisodeMessage>(this, async message => await PlayEpisodeAsync(message.Episode));
        }

        public async Task PauseAsync()
        {
            await DispatcherHelper.RunAsync(() =>
            {
                player.Pause();
                systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
            });
        }

        public async Task PlayEpisodeAsync(Episode episode)
        {
            if (systemControls.PlaybackStatus == MediaPlaybackStatus.Playing)
                player.Stop();

            if (episode.Path.StartsWith("http://") ||
                episode.Path.StartsWith("https://"))
                PlayFromNetwork(episode);
            else
                await PlayLocalEpisodeAsync(episode);

            UpdateSystemControls(episode);
        }

        public void SetupBackgroundAudio()
        {
            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.IsStopEnabled = true;
            systemControls.IsEnabled = true;
            systemControls.DisplayUpdater.Type = MediaPlaybackType.Music;
            systemControls.ButtonPressed += SystemControlsButtonPressed;
        }

        private async void SystemControlsButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    await ResumeAsync();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    await PauseAsync();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    await StopAsync();
                    break;
                case SystemMediaTransportControlsButton.Record:
                    break;
                case SystemMediaTransportControlsButton.FastForward:
                    break;
                case SystemMediaTransportControlsButton.Rewind:
                    break;
                case SystemMediaTransportControlsButton.Next:
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    break;
                case SystemMediaTransportControlsButton.ChannelUp:
                    break;
                case SystemMediaTransportControlsButton.ChannelDown:
                    break;
                default:
                    break;
            }
        }

        public async Task StopAsync()
        {
            await DispatcherHelper.RunAsync(() =>
            {
                player.Stop();
            });
        }

        private async Task PlayLocalEpisodeAsync(Episode episode)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(episode.Path);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                player.SetSource(stream, file.ContentType);
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    ;
                }
            }
        }

        private void PlayFromNetwork(Episode episode)
        {
            player.Source = new Uri(episode.Path);
            player.Play();
        }

        public async Task ResumeAsync()
        {
            await DispatcherHelper.RunAsync(() =>
            {
                player.Play();
                systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            });
        }

        public void UpdateSystemControls(Episode episode)
        {
            systemControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(episode.ImageUrl);
            systemControls.DisplayUpdater.MusicProperties.AlbumArtist = episode.Podcast.Author;
            systemControls.DisplayUpdater.MusicProperties.Artist = episode.Author;
            systemControls.DisplayUpdater.MusicProperties.Title = episode.Title;
            systemControls.DisplayUpdater.Update();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
        }
    }
}
