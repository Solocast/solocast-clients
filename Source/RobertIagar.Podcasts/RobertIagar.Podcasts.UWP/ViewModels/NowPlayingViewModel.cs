using GalaSoft.MvvmLight;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class NowPlayingViewModel : ViewModelBase
    {
        private IPlayService playService;
        private Episode nowPlayingEpisode;

        public NowPlayingViewModel(IPlayService playService)
        {
            this.playService = playService;
            MessengerInstance.Register<PlayEpisodeMessage>(this, message => PlayEpisode(message.Episode));
        }

        private void PlayEpisode(Episode episode)
        {
            NowPlayingEpisode = episode;
        }

        public Episode NowPlayingEpisode
        {
            get { return nowPlayingEpisode; }
            set { Set(nameof(NowPlayingEpisode), ref nowPlayingEpisode, value); }
        }
    }
}
