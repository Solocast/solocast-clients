﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class NowPlayingViewModel : ViewModelBase
    {
        private IPlayService playService;
        private Episode nowPlayingEpisode;
        private DispatcherTimer timer;
        private int absvalue;
        private TimeSpan elapsedTime;
        private TimeSpan totalTime;
        private double progress;
        private string playPauseLabel;
        private IconElement playPauseIcon;
        private bool canPlayPause = false;

        public NowPlayingViewModel(IPlayService playService)
        {
            this.playService = playService;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += TimerTick;

            this.StopCommand = new RelayCommand(async () => await StopAsync());
            this.PlayPauseCommand = new RelayCommand(async () => await PlayPauseAsync(), CanPlayPause);
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

        public Episode NowPlayingEpisode
        {
            get { return nowPlayingEpisode; }
            set
            {
                Set(nameof(NowPlayingEpisode), ref nowPlayingEpisode, value);
                (PlayPauseCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public TimeSpan ElapsedTime
        {
            get { return elapsedTime; }
            set { Set(nameof(ElapsedTime), ref elapsedTime, value); }
        }

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set { Set(nameof(TotalTime), ref totalTime, value); }
        }

        public double Progress
        {
            get { return progress; }
            set
            {
                Set(nameof(Progress), ref progress, value);
                playService.GoToTime(value);
            }
        }

        public ICommand StopCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand NextCommand { get; }

        private void TimerTick(object sender, object e)
        {
            absvalue = (int)Math.Round(playService.TotalTime.TotalSeconds, MidpointRounding.AwayFromZero);
            ElapsedTime = playService.Position;
            TotalTime = TimeSpan.FromSeconds(absvalue);
            progress = (ElapsedTime.TotalSeconds * 100) / TotalTime.TotalSeconds;
            RaisePropertyChanged(nameof(Progress));
            UpdatePlayPauseLabelAndIcon();
        }

        private void UpdatePlayPauseLabelAndIcon()
        {
            switch (playService.CurrentState)
            {
                case MediaElementState.Closed:
                    break;
                case MediaElementState.Opening:
                    break;
                case MediaElementState.Buffering:
                    break;
                case MediaElementState.Playing:
                    PlayPauseIcon = new SymbolIcon(Symbol.Pause);
                    PlayPauseLabel = "Pause";
                    canPlayPause = true;
                    break;
                case MediaElementState.Paused:
                    PlayPauseIcon = new SymbolIcon(Symbol.Play);
                    PlayPauseLabel = "Resume";
                    canPlayPause = true;
                    break;
                case MediaElementState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private void PlayEpisode(Episode episode)
        {
            NowPlayingEpisode = episode;
            if (timer.IsEnabled)
                timer.Stop();

            timer.Start();

            PlayPauseLabel = "Pause";
            canPlayPause = true;
            (PlayPauseCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private async Task PlayPauseAsync()
        {
            switch (playService.CurrentState)
            {
                case MediaElementState.Closed:
                    break;
                case MediaElementState.Opening:
                    break;
                case MediaElementState.Buffering:
                    break;
                case MediaElementState.Playing:
                    await playService.PauseAsync();
                    break;
                case MediaElementState.Paused:
                    await playService.ResumeAsync();
                    break;
                case MediaElementState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private bool CanPlayPause()
        {
            return canPlayPause;
        }

        private async Task StopAsync()
        {
            await playService.StopAsync();
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