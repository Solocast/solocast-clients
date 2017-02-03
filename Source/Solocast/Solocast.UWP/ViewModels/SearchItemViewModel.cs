using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using Solocast.UWP.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solocast.UWP.ViewModels
{
	public class SearchItemViewModel : ViewModelBase
	{
		private IPodcastService podcastService;

		public SearchItemViewModel(SearchPodcast podcast)
		{
			podcastService = SimpleIoc.Default.GetInstance<IPodcastService>();
			Podcast = podcast;
			SubscribeCommand = new RelayCommand(async () => await SubscribeAsync());
		}

		public ICommand SubscribeCommand { get; }

		public SearchPodcast Podcast { get; }

		private async Task SubscribeAsync()
		{
			var podcast = await podcastService.GetPodcastAsync(Podcast.FeedUrl.ToString());

			MessengerInstance.Send(new SubcribeToPodcastMessage(podcast));
		}
	}
}
