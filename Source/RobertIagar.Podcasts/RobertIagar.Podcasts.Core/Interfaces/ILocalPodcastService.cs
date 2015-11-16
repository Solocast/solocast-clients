using RobertIagar.Podcasts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface ILocalPodcastService
    {
        Task SaveAsync(Podcast entity);
        Task SaveAsync(IEnumerable<Podcast> entities);
        Task<IList<Podcast>> LoadAsync();
    }
}
