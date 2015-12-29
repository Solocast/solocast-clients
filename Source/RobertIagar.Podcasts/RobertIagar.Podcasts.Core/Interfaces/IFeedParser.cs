using RobertIagar.Podcasts.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface IFeedParaseService
    {
        Task<dynamic> GetChannelNodeAsync(string feedUrl);
        Task<dynamic> GetChannelNodeAsync(Uri feedUrl);
        Podcast GetPodcast(dynamic json, string feedUrl);
    }
}
