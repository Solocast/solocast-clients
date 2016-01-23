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
        void GoToTime(int seconds);

        int Position { get; }
        int TotalSeconds { get; }
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

        public void GoToTime(int seconds)
        {
            mediator.Position = TimeSpan.FromSeconds(seconds);
        }

        public int Position
        {
            get { return (int)mediator.Position.TotalSeconds; }
        }

        public int TotalSeconds
        {
            get { return (int)mediator.TotalTime.TotalSeconds; }
        }

        public MediaPlayerState CurrentState
        {
            get { return mediator.CurrentState; }
        }
    }
}
