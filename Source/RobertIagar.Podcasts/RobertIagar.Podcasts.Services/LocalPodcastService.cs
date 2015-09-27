using Newtonsoft.Json;
using RobertIagar.Podcasts.Core.Entities;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RobertIagar.Podcasts.Services
{
    public class LocalPodcastService : LocalStorageService<Podcast>
    {
        public LocalPodcastService()
            : base("podcasts.json")
        {
        }
    }
}
