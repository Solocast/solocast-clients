using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace Solocast.Core.Interfaces
{
    public interface IBackgroundMediaPlayerMediator
    {
        Task SendMessageToBackgroundAsync<T>(T message);
        TimeSpan TotalTime { get; }
        TimeSpan Position { get; set; }
        MediaPlayerState CurrentState { get; }

        void Pause();
        void Stop();
        void Resume();
    }
}
