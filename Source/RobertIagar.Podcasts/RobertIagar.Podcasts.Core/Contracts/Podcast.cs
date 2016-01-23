using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Contracts
{
    public class Podcast : IEquatable<Podcast>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual IList<Episode> Episodes { get; set; }

        public Podcast(string name,
            string description,
            string author,
            string feedUrl,
            string imageUrl,
            DateTime dateAdded)
        {
            this.Title = name;
            this.Description = description;
            this.Author = author;
            this.FeedUrl = new Uri(feedUrl);
            this.ImageUrl = new Uri(imageUrl);
            this.DateAdded = dateAdded;
        }

        public void SetEpisodes(IEnumerable<Episode> episode)
        {
            this.Episodes = episode.ToList();
        }

        public bool Equals(Podcast other)
        {
            //not the best logic, but it will do
            if (this.FeedUrl == other.FeedUrl)
            {
                if (this.Episodes.Count == other.Episodes.Count)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
