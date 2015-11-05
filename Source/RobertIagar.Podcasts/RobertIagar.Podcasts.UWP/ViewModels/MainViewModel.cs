using GalaSoft.MvvmLight;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.UWP.Models;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IPodcastService podcastService;
        private Podcast podcast;

        public MainViewModel(IPodcastService podcastService)
            : base()
        {
            this.podcastService = podcastService;
            this.GetPodcastCommand = new RelayCommand(async () => await this.GetPodcast("https://www.monstercat.com/podcast/feed.xml"));
        }

        public Podcast Podcast
        {
            get { return podcast; }
            set
            {
                Set<Podcast>(() => this.Podcast, ref podcast, value);
            }
        }

        public ICommand GetPodcastCommand { get; private set; }

        public async Task GetPodcast(string feedUrl)
        {
            var corePodcast = await podcastService.GetPodcastAsync(feedUrl);
            this.Podcast = corePodcast.ToPodcastModel();
            corePodcast.Episodes.ForEach(e => this.Podcast.Episodes.Add(e.ToEpisodeModel()));
        }
    }
}