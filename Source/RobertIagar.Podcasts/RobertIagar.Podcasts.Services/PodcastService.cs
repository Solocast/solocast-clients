using RobertIagar.Podcasts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobertIagar.Podcasts.Core.Entities;

namespace RobertIagar.Podcasts.Services
{
    public class PodcastService : IPodcastService
    {
        private IFeedParser feedParser;
        private ILocalStorageService<Podcast> storageService;

        public PodcastService(IFeedParser feedParser, ILocalStorageService<Podcast> storageService)
        {
            this.feedParser = feedParser;
            this.storageService = storageService;
        }

        public async Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            dynamic feed = await feedParser.GetChannelNodeAsync(feedUrl);
            var podcast = feedParser.GetPodcast(feed);
            return podcast;
        }

        public async Task<IEnumerable<Podcast>> GetPodcastsAsync()
        {
            var podcasts = await storageService.LoadAsync();
            return podcasts;
        }

        public async Task<IEnumerable<Episode>> GetNewEpisodesAsync(Podcast podcast)
        {
            dynamic feed = await feedParser.GetChannelNodeAsync(podcast.FeedUrl);
            Podcast newPodcast = feedParser.GetPodcast(feed);
            var newEpisodes = new List<Episode>();

            foreach(var episode in newPodcast.Episodes)
            {
                if (!podcast.Episodes.Contains(episode))
                {
                    newEpisodes.Add(episode);
                }
            }

            return newEpisodes;
        }

        public async Task SavePodcastAsync(Podcast podcast)
        {
            await storageService.SaveAsync(podcast);
        }

        public async Task SavePodcastsAsync(IEnumerable<Podcast> podcasts)
        {
            await storageService.SaveAsync(podcasts);
        }

        public Task<IEnumerable<Podcast>> SearchPodcast(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
