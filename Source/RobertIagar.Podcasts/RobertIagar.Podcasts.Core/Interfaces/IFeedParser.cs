using RobertIagar.Podcasts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface IFeedParser
    {
        Task<dynamic> GetChannelNodeAsync(string feedUrl);
        Task<dynamic> GetChannelNodeAsync(Uri feedUrl);
        Podcast GetPodcast(dynamic json);
    }
}
