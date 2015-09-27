using RobertIagar.Podcasts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface IPodcastService
    {
        Task<Podcast> GetPodcastAsync(string feedUrl);
        Task<IEnumerable<Podcast>> GetPodcastsAsync();
        Task<IEnumerable<Podcast>> SearchPodcast(string searchString);
        Task<IEnumerable<Episode>> GetNewEpisodesAsync(Podcast podcast);
        Task SavePodcastAsync(Podcast podcast);
        Task SavePodcastsAsync(IEnumerable<Podcast> podcasts);
    }
}
