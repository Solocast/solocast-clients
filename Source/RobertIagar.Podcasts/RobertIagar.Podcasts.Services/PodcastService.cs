using RobertIagar.Podcasts.Core.Services;
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
        public Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            return GetPodcastAsync(new Uri(feedUrl));
        }

        public Task<Podcast> GetPodcastAsync(Uri feedUrl)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Episode>> GetNewEpisodesAsync(Podcast podcast)
        {
            throw new NotImplementedException();
        }

        public Task<Podcast> GetPodcastAsync(Guid podcastId)
        {
            throw new NotImplementedException();
        }

        public Task SavePodcastAsync(Podcast podcast)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Podcast>> SearchPodcast(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
