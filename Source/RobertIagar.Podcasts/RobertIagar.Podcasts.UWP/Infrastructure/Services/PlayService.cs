using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RobertIagar.Podcasts.Core.Contracts;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Services.Extensions;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Services
{
    public interface IPlayService
    {
        Task PlayEpisodeAsync(Episode episode);
        void Pause();
        void Stop();
        void Resume();
        void GoToTime(double progress);

        TimeSpan Position { get; }
        TimeSpan TotalTime { get; }
        MediaPlayerState CurrentState { get; }
    }

    public class PlayService : IPlayService
    {
        private IBackgroundMediaPlayerMediator mediator;

        public PlayService(IBackgroundMediaPlayerMediator mediator)
        {
            this.mediator = mediator;
            Messenger.Default.Register<PlayEpisodeMessage>(this, async message => await PlayEpisodeAsync(message.Episode));
        }

        public void Pause()
        {
            mediator.Pause();
        }

        public async Task PlayEpisodeAsync(Episode episode)
        {
            await mediator.SendMessageToBackgroundAsync(episode);
        }

        public void Stop()
        {
            mediator.Stop();
        }

        public void Resume()
        {
            mediator.Resume();
        }

        public void GoToTime(double progress)
        {
            if (progress != 0)
            {
                var newPosition = (mediator.TotalTime.TotalSeconds * progress) / 100;
                mediator.Position = TimeSpan.FromSeconds(newPosition);
            }
        }

        public TimeSpan Position
        {
            get { return mediator.Position; }
        }

        public TimeSpan TotalTime
        {
            get { return mediator.TotalTime; }
        }

        public MediaPlayerState CurrentState
        {
            get { return mediator.CurrentState; }
        }
    }
}
