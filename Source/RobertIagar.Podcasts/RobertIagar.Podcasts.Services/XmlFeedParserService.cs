using RobertIagar.Podcasts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobertIagar.Podcasts.Core.Contracts;
using System.Net.Http;

namespace RobertIagar.Podcasts.Services
{
    public class XmlFeedParserService : IFeedParaseService
    {
        public Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            throw new NotImplementedException();
        }
    }
}
