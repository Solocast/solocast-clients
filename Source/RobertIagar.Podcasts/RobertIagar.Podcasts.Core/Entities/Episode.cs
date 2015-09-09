using Newtonsoft.Json;
using System;

namespace RobertIagar.Podcasts.Core.Entities
{
    public class Episode
    {
        public string Name { get; set; }
        public string WebPath { get; set; }
        public bool IsLocal { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public Uri ImageUrl { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual Podcast Podcast { get; set; }
    }
}