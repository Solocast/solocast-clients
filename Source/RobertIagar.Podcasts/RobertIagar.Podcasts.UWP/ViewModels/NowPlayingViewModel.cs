using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RobertIagar.Podcasts.Core.Contracts;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class NowPlayingViewModel : ViewModelBase
    {
        private IPlayService playService;
        private DispatcherTimer timer;
        private int elapsedSeconds;
        private TimeSpan totalTime;
        private string playPauseLabel;
        private IconElement playPauseIcon;
        private bool canPlayPause = false;
        private string author = nameof(Author);
        private string title = nameof(Title);
        private string description = nameof(Description);
        private string subtitle = nameof(Subtitle);
        private Uri imageUrl;

        public NowPlayingViewModel(IPlayService playService)
        {
            this.playService = playService;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(500);
            this.timer.Tick += TimerTick;

            this.StopCommand = new RelayCommand(Stop);
            this.PlayPauseCommand = new RelayCommand(PlayPause, CanPlayPause);
            this.NextCommand = new RelayCommand(Next, CanNext);
            MessengerInstance.Register<PlayEpisodeMessage>(this, message => PlayEpisode(message.Episode));
        }

        public string PlayPauseLabel
        {
            get { return playPauseLabel; }
            set { Set(nameof(PlayPauseLabel), ref playPauseLabel, value); }
        }

        public IconElement PlayPauseIcon
        {
            get { return playPauseIcon; }
            set { Set(nameof(PlayPauseIcon), ref playPauseIcon, value); }
        }

        public int ElapsedSeconds
        {
            get { return elapsedSeconds; }
            set
            {
                timer.Stop();
                playService.GoToTime(value);
                Set(nameof(ElapsedSeconds), ref elapsedSeconds, value);
                timer.Start();
            }
        }

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set { Set(nameof(TotalTime), ref totalTime, value); }
        }


        public string Author
        {
            get { return author; }
            set { Set(nameof(Author), ref author, value); }
        }

        public string Title
        {
            get { return title; }
            set { Set(nameof(Title), ref title, value); }
        }

        public string Description
        {
            get { return description; }
            set { Set(nameof(Description), ref description, value); }
        }

        public string Subtitle
        {
            get { return subtitle; }
            set { Set(nameof(subtitle), ref subtitle, value); }
        }

        public Uri ImageUrl
        {
            get { return imageUrl; }
            set { Set(nameof(ImageUrl), ref imageUrl, value); }
        }

        public ICommand StopCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand NextCommand { get; }

        private void TimerTick(object sender, object e)
        {
            TotalTime = playService.TotalTime;
            elapsedSeconds = (int)playService.Position.TotalSeconds;
            RaisePropertyChanged(nameof(ElapsedSeconds));
            var appview = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appview.Title = TimeSpan.FromSeconds(elapsedSeconds).ToString("c");
            UpdatePlayPauseLabelAndIcon();
        }

        private void UpdatePlayPauseLabelAndIcon()
        {
            switch (playService.CurrentState)
            {
                case Windows.Media.Playback.MediaPlayerState.Closed:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Opening:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Buffering:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Playing:
                    PlayPauseIcon = new SymbolIcon(Symbol.Pause);
                    PlayPauseLabel = "Pause";
                    canPlayPause = true;
                    break;
                case Windows.Media.Playback.MediaPlayerState.Paused:
                    PlayPauseIcon = new SymbolIcon(Symbol.Play);
                    PlayPauseLabel = "Resume";
                    canPlayPause = true;
                    break;
                case Windows.Media.Playback.MediaPlayerState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private void PlayEpisode(Episode episode)
        {
            Title = episode.Title;
            Author = episode.Author;
            ImageUrl = episode.ImageUrl;
            Description = episode.Summary;
            Subtitle = episode.Subtitle;

            if (timer.IsEnabled)
                timer.Stop();

            timer.Start();

            PlayPauseLabel = "Pause";
            canPlayPause = true;
        }

        private void PlayPause()
        {
            switch (playService.CurrentState)
            {
                case Windows.Media.Playback.MediaPlayerState.Closed:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Opening:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Buffering:
                    break;
                case Windows.Media.Playback.MediaPlayerState.Playing:
                    playService.Pause();
                    break;
                case Windows.Media.Playback.MediaPlayerState.Paused:
                    playService.Resume();
                    break;
                case Windows.Media.Playback.MediaPlayerState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private bool CanPlayPause()
        {
            return canPlayPause;
        }

        private void Stop()
        {
            playService.Stop();
        }

        private void Next()
        {
            throw new NotImplementedException();
        }

        private bool CanNext()
        {
            try
            {
                Next();
            }
            catch (NotImplementedException)
            {
                return false;
            }

            return true;
        }
    }
}
