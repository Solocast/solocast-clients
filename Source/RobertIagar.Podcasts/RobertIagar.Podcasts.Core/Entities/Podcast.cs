using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Entities
{
    public class Podcast
    {
        public string Name { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid PodcastId { get; set; }

        public virtual IList<Episode> Episodes { get; set; }
    }
}
