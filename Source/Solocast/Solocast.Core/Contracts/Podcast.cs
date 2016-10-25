using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.Core.Contracts
{
    public class Podcast : IEquatable<Podcast>
    {
        public int PodcastId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string FeedUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public List<Episode> Episodes { get; set; }

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
            this.FeedUrl = feedUrl;
            this.ImageUrl = imageUrl;
            this.DateAdded = dateAdded;
        }

        public Podcast()
        {

        }

        public void SetEpisodes(IEnumerable<Episode> episode)
        {
            this.Episodes = episode.OrderByDescending(e => e.Published).ToList();
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
