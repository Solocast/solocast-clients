using RobertIagar.Podcasts.Core.Entities;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace RobertIagar.Podcasts.Services
{


    public class BackgroundMediaPlayerMediator : IBackgroundMediaPlayerMediator
    {
        private Dictionary<object, bool> confirmations;
        private SemaphoreSlim signal = new SemaphoreSlim(0, 1);

        public BackgroundMediaPlayerMediator()
        {
            this.confirmations = new Dictionary<object, bool>();

            BackgroundMediaPlayer.MessageReceivedFromBackground += MessageRecieved;
        }

        public TimeSpan TotalTime
        {
            get
            {
                try { return BackgroundMediaPlayer.Current.NaturalDuration; }
                catch { return new TimeSpan(); }
            }
        }

        public TimeSpan Position
        {
            get
            {
                try { return BackgroundMediaPlayer.Current.Position; }
                catch { return new TimeSpan(); }
            }
            set
            {
                BackgroundMediaPlayer.Current.Position = value;
            }
        }

        public MediaPlayerState CurrentState
        {
            get
            {
                try { return BackgroundMediaPlayer.Current.CurrentState; }
                catch { return MediaPlayerState.Closed; }
            }
        }

        private void MessageRecieved(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            var episode = e.Data.ToObject<Episode>();
            confirmations[episode] = true;
            signal.Release();
            confirmations.Clear();
        }

        public async Task SendMessageToBackgroundAsync<T>(T message)
        {
            if (!confirmations.ContainsKey(message))
                confirmations.Add(message, false);
            BackgroundMediaPlayer.SendMessageToBackground(message.ToValueSet());
            var completed = await signal.WaitAsync(10000);

            if (!completed)
                await SendMessageToBackgroundAsync(message);
        }

        public void Pause()
        {
            BackgroundMediaPlayer.Current.Pause();
        }

        public void Stop()
        {
            BackgroundMediaPlayer.Current.Pause();
            BackgroundMediaPlayer.Current.Position = new TimeSpan(0);
        }

        public void Resume()
        {
            BackgroundMediaPlayer.Current.Play();
        }
    }
}
