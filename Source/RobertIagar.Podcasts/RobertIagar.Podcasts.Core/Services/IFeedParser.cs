using RobertIagar.Podcasts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Services
{
    public interface IFeedParser
    {
        Task<dynamic> GetFeedAsync(Uri feedUrl);
        Podcast GetPodcast(dynamic json);
    }
}
