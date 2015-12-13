using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RobertIagar.Podcasts.Core.Exceptions;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace RobertIagar.Podcasts.UWP.ViewModels
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
            this.AddPodcastCommand = new RelayCommand(async () => await this.AddPodcastAsync(), () => this.CanAddPodcast());
            this.loadedPodcasts = false;

            MessengerInstance.Register<LoadPodcastsMessage>(this, async message => await LoadPodcastsAsync());
            MessengerInstance.Register<DeletePodcastMessage>(this, async message => await DeletePodcastAsync(message));
            MessengerInstance.Register<SavePodcastsMessage>(this, async message => await SavePodcastsAsync());
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


        public async Task AddPodcastAsync()
        {
            try
            {
                var corePodcast = await podcastService.GetPodcastAsync(feedUrl);
                var podcastModel = corePodcast.ToPodcastViewModel();
                podcastModel.LoadEpisodeViewModels();

                if (!podcasts.Contains(podcastModel))
                {
                    podcasts.Add(podcastModel);
                    await podcastService.SavePodcastAsync(corePodcast);
                }
                else
                {

                }

                FeedUrl = string.Empty;
            }
            catch (GetPodcastException ex)
            {
                var result = await dialogService.ShowDialogAsync(ex.Message, "Error!", new[]
                {
                    new UICommand("Retry") { Id = 0 },
                    new UICommand("Cancel") { Id = 1 }
                }, 0);

                if ((int)result.Id == 0)
                {
                    await AddPodcastAsync();
                }
                else
                {
                }
            }
        }

        private bool CanAddPodcast()
        {
            Uri uriResult;
            bool result = Uri.TryCreate(FeedUrl, UriKind.Absolute, out uriResult);
            return result;
        }

        private async Task LoadPodcastsAsync()
        {
            if (!loadedPodcasts)
            {
                var podcasts = await podcastService.GetPodcastsAsync();
                podcasts.ForEach(p => this.podcasts.Add(p.ToPodcastViewModel()));
                this.podcasts.ForEach(p => p.LoadEpisodeViewModels());
                loadedPodcasts = true;
            }
        }

        private async Task DeletePodcastAsync(DeletePodcastMessage message)
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
                await podcastService.SavePodcastsAsync(podcasts.Select(p => p.Podcast.Core));
            }
        }

        private async Task SavePodcastsAsync()
        {
            await podcastService.SavePodcastsAsync(podcasts.Select(p => p.Podcast.Core));
        }

        public void ItemClickCommand(object sender, ItemClickEventArgs parameters)
        {
            var podcastVm = parameters.ClickedItem as PodcastViewModel;
            this.navigationService.NavigateTo(nameof(PodcastDetailsViewModel), podcastVm);
        }
    }
}