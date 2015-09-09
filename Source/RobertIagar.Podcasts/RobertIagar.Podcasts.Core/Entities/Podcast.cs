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
        public string Description { get; set; }
        public string Author { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual IList<Episode> Episodes { get; set; }

        public Podcast(string name,
            string description,
            string author,
            Uri feedUrl,
            Uri imageUrl,
            DateTime dateAdded)
        {
            this.Name = name;
            this.Description = description;
            this.Author = author;
            this.FeedUrl = feedUrl;
            this.ImageUrl = imageUrl;
            this.DateAdded = dateAdded;
        }
    }
}
