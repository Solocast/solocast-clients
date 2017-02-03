using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Solocast.Core.Exceptions;
using Solocast.Core.Interfaces;
using Solocast.UWP.Infrastructure.Extensions;
using Solocast.UWP.Infrastructure.Messages;
using Solocast.UWP.Infrastructure.Services;
using Solocast.UWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Solocast.UWP.ViewModels
{
    public class PodcastsViewModel : ViewModelBase
    {
        private IPodcastService podcastService;
        private INavigationService navigationService;
        private IMessageDialogService dialogService;
        private string feedUrl;
        private ObservableCollection<PodcastViewModel> podcasts;
        private bool loadedPodcasts;

        public PodcastsViewModel(IPodcastService podcastService, INavigationService navigationService, IMessageDialogService dialogService)
            : base()
        {
            this.podcastService = podcastService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.podcasts = new ObservableCollection<PodcastViewModel>();
            this.AddPodcastCommand = new RelayCommand(AddPodcastAsync,CanAddPodcast);
            this.loadedPodcasts = false;

            MessengerInstance.Register<LoadPodcastsMessage>(this, message => LoadPodcastsAsync());
            MessengerInstance.Register<DeletePodcastMessage>(this, message => DeletePodcastAsync(message));
            MessengerInstance.Register<CheckForNewEpsiodesMessage>(this, message => CheckForNewEpisodesAsync());
			MessengerInstance.Register<SubcribeToPodcastMessage>(this, async message => await SubscribeToPodcastAsync(message.Podcast));
        }

        public IList<PodcastViewModel> Podcasts { get { return podcasts; } }

        public string FeedUrl
        {
            get { return feedUrl; }
            set
            {
                Set(nameof(FeedUrl), ref feedUrl, value);
                ((RelayCommand)AddPodcastCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPodcastCommand { get; private set; }


        public async void AddPodcastAsync()
        {
            try
			{
				var corePodcast = await podcastService.GetPodcastAsync(feedUrl);
				await SubscribeToPodcastAsync(corePodcast);

				FeedUrl = string.Empty;
			}
			catch (GetPodcastException ex)
            {
                var result = await dialogService.ShowDialogAsync(ex.Message, "Error!", new[]
                {
                    new UICommand("Retry") { Id = 0 },
                    new UICommand("Cancel") { Id = 1 },
                    new UICommand("test") {Id =2 }
                }, 0);

                if ((int)result.Id == 0)
                {
                    AddPodcastAsync();
                }
                else
                {
                }
            }
        }

		private async Task SubscribeToPodcastAsync(Core.Contracts.Podcast corePodcast)
		{
			var podcastModel = new PodcastViewModel(corePodcast);

			if (!podcasts.Contains(podcastModel))
			{
				podcasts.Add(podcastModel);
				await podcastService.SavePodcastAsync(corePodcast);
			}
		}

		private bool CanAddPodcast()
        {
            Uri uriResult;
            bool result = Uri.TryCreate(FeedUrl, UriKind.Absolute, out uriResult);
            return result;
        }

        private async void LoadPodcastsAsync()
        {
			Debug.WriteLine("Loading podcasts...");
            if (!loadedPodcasts)
            {
                var podcasts = await podcastService.GetPodcastsAsync();
                podcasts.ForEach(p => this.podcasts.Add(new PodcastViewModel(p)));
                loadedPodcasts = true;
            }
			Debug.WriteLine("Loaded podcasts");
		}

		private async void DeletePodcastAsync(DeletePodcastMessage message)
        {
            var podcastVm = message.PodcastViewModel;
            var podcast = podcastVm.Podcast;
            var dialogResult = await dialogService.ShowDialogAsync($"Do you really want to delete {podcast.Title}?", $"Delete {podcast.Title}?", new[]
            {
                new UICommand("Yes") { Id = 0 },
                new UICommand("No") { Id = 1 },
            }, 0);

            if ((int)dialogResult.Id == 0)
            {
                podcasts.Remove(podcastVm);
            }
        }

        private async void CheckForNewEpisodesAsync()
        {
			Debug.WriteLine("Checking for epiodes...");
			foreach (var podcast in podcasts)
            {
				Debug.WriteLine($"Checking episodes for {podcast.Podcast.Title}");
                try
                {
                    var newEpisodes = await podcastService.GetNewEpisodesAsync(podcast.Podcast);
                    var episodes = newEpisodes.Select(e => new EpisodeViewModel(e));
                    episodes.ForEach(e =>
                    {
                        podcast.Episodes.Insert(0, e);
                        podcast.Podcast.Episodes.Insert(0, e.Episode);
                    });
                }
                catch (GetPodcastException ex)
                {
                    //would be a nice idea to log this
                }
            }
			Debug.WriteLine("Check for episodes done...");
        }


        public void ItemClickCommand(object sender, ItemClickEventArgs parameters)
        {
            var podcastVm = parameters.ClickedItem as PodcastViewModel;
            this.navigationService.NavigateTo(nameof(PodcastDetailsViewModel), podcastVm);
        }
    }
}