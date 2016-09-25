using Solocast.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.Core.Interfaces
{
    public interface IFeedParaseService
    {
        Task<Podcast> GetPodcastAsync(string feedUrl);
    }
}
