using RobertIagar.Podcasts.Core.Entities;
using RobertIagar.Podcasts.Services;
using RobertIagar.Podcasts.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;

namespace RobertIagar.Podcasts.BackgroundTasks
{
    public sealed class AudioBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral defferal;
        private ManualResetEvent backgroundTaskStarted = new ManualResetEvent(false);
        private SystemMediaTransportControls smtc;
        private bool playbackStartedPreviously = false;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            RegisterSystemMediaTransportControlsEvents();
            RegisterBackgroundMediaPlayerEvents();

            defferal = taskInstance.GetDeferral();
            backgroundTaskStarted.Set();
            taskInstance.Task.Completed += Completed;
            taskInstance.Canceled += OnCanceled;
        }

        private void RegisterBackgroundMediaPlayerEvents()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += OnCurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromForeground += OnMessageReceivedFromForeground;
        }

        private async void OnMessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            var data = e.Data;
            var episode = e.Data.ToObject<Episode>();

            if (episode != null)
            {
                BackgroundMediaPlayer.SendMessageToForeground(episode.ToValueSet());
                await PlayEpisodeAsync(episode);
                UpdateSystemControls(episode);
            }
        }

        private async Task PlayEpisodeAsync(Episode episode)
        {
            if (episode.Path.StartsWith("https://") ||
                episode.Path.StartsWith("http://"))
                BackgroundMediaPlayer.Current.SetUriSource(new Uri(episode.Path));
            else
            {
                var file = await StorageFile.GetFileFromPathAsync(episode.Path);
                BackgroundMediaPlayer.Current.SetFileSource(file);
            }

            BackgroundMediaPlayer.Current.Play();
        }

        private void OnCurrentStateChanged(MediaPlayer sender, object args)
        {
            //throw new NotImplementedException();
        }

        private void RegisterSystemMediaTransportControlsEvents()
        {
            smtc = BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            smtc.IsEnabled = true;
            smtc.IsPauseEnabled = true;
            smtc.IsStopEnabled = true;
            smtc.IsPlayEnabled = true;
            smtc.ButtonPressed += OnButtonPressed;
        }

        private void OnButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    bool result = backgroundTaskStarted.WaitOne(5000);
                    if (!result)
                        return;
                    StartPlayback();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    BackgroundMediaPlayer.Current.Pause();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    BackgroundMediaPlayer.Shutdown();
                    break;
            }
        }

        private void StartPlayback()
        {
            if (playbackStartedPreviously)
                //resume playback
                BackgroundMediaPlayer.Current.Play();
            else
            {
                playbackStartedPreviously = true;

                //find last index and start from there.
            }
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            try
            {
                backgroundTaskStarted.Reset();
                UnregisterHandlers();
            }
            catch
            {

            }

            defferal.Complete();
        }

        private void UnregisterHandlers()
        {
            BackgroundMediaPlayer.MessageReceivedFromForeground -= OnMessageReceivedFromForeground;
            smtc.ButtonPressed -= OnButtonPressed;
            BackgroundMediaPlayer.Shutdown();
        }

        private void Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            defferal.Complete();
        }

        private void UpdateSystemControls(Episode episode)
        {
            smtc.PlaybackStatus = MediaPlaybackStatus.Playing;
            smtc.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(episode.ImageUrl);
            smtc.DisplayUpdater.Type = MediaPlaybackType.Music;
            smtc.DisplayUpdater.MusicProperties.Artist = episode.Author;
            smtc.DisplayUpdater.MusicProperties.Title = episode.Title;
            smtc.DisplayUpdater.Update();
        }
    }
}
