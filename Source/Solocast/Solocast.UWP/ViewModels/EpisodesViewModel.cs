using GalaSoft.MvvmLight;
using Solocast.Core.Interfaces;
using Solocast.UWP.Infrastructure.Extensions;
using Solocast.UWP.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Solocast.UWP.ViewModels
{
    public class EpisodesViewModel : ViewModelBase
    {
        IPodcastService podcastService;
        ObservableCollection<EpisodeViewModel> episodes;

        public EpisodesViewModel(IPodcastService podcastService)
        {
            this.podcastService = podcastService;
            this.episodes = new ObservableCollection<EpisodeViewModel>();

            MessengerInstance.Register<LoadEpisodesMessage>(this, message => LoadEpisodeAsync());
        }

        private async void LoadEpisodeAsync()
        {
            episodes.Clear();
            var tempEpisodes = new List<EpisodeViewModel>();
            var podcasts = await podcastService.GetPodcastsAsync();
            foreach (var podcast in podcasts)
            {
                var podcastVm = new PodcastViewModel(podcast);
                tempEpisodes.AddRange(podcastVm.Episodes);
            }
            tempEpisodes = tempEpisodes.OrderByDescending(ep => ep.Episode.Published).ToList();
            foreach (var episode in tempEpisodes)
            {
                episodes.Add(episode);
            }
        }

        public IList<EpisodeViewModel> Episodes => episodes;

		public void PlayEpisode(object sender, ItemClickEventArgs parameters)
		{
			var episodeVm = parameters.ClickedItem as EpisodeViewModel;

			MessengerInstance.Send(new PlayEpisodeMessage(episodeVm.Episode));
		}
    }
}
