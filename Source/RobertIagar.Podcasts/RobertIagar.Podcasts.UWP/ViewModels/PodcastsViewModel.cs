using GalaSoft.MvvmLight;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.UWP.Models;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class PodcastsViewModel : ViewModelBase
    {
        private IPodcastService podcastService;
        private string feedUrl;
        private ObservableCollection<Podcast> podcasts;

        public PodcastsViewModel(IPodcastService podcastService)
            : base()
        {
            this.podcastService = podcastService;
            this.podcasts = new ObservableCollection<Podcast>();
            this.GetPodcastCommand = new RelayCommand(async () => await this.GetPodcastAsync(), () => this.CanGetPodcast());

            MessengerInstance.Register<LoadPodcastsMessage>(this, async message => await LoadPodcastsAsync(message));
        }

        public IList<Podcast> Podcasts { get { return podcasts; } }

        public string FeedUrl
        {
            get { return feedUrl; }
            set
            {
                Set(nameof(FeedUrl), ref feedUrl, value);
                ((RelayCommand)GetPodcastCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand GetPodcastCommand { get; private set; }

        public async Task GetPodcastAsync()
        {
            var corePodcast = await podcastService.GetPodcastAsync(feedUrl);
            podcasts.Add(corePodcast.ToPodcastModel());
            await podcastService.SavePodcastAsync(corePodcast);
        }

        private bool CanGetPodcast()
        {
            return !string.IsNullOrEmpty(FeedUrl);
        }

        private async Task LoadPodcastsAsync(LoadPodcastsMessage message)
        {
            var podcasts = await podcastService.GetPodcastsAsync();
            podcasts.ForEach(p => this.podcasts.Add(p.ToPodcastModel()));
            MessengerInstance.Unregister(this);
        }
    }
}