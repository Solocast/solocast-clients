using Newtonsoft.Json;
using System;

namespace RobertIagar.Podcasts.Core.Contracts
{
    public class Episode :  IEquatable<Episode>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Path { get; set; }
        public string WebPath { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Guid { get; set; }
        public DateTime Published { get; set; }
        public Uri ImageUrl { get; set; }
        public bool IsLocal { get { return WebPath != Path; } }

        [JsonIgnore]
        public virtual Podcast Podcast { get; set; }

        public Episode(string title,
            string subtitle,
            string path,
            string author,
            string summary,
            string published,
            string imageUrl,
            string guid)
        {
            this.Title = title;
            this.Subtitle = subtitle;
            this.Path = this.WebPath = path;
            this.Author = author;
            this.Summary = summary;
            this.Published = published.ToDateTime();
            this.ImageUrl = new Uri(imageUrl);
            this.Guid = guid;
        }

        [Obsolete]
        public Episode()
        {
        }

        public void SetPodcast(Podcast podcast)
        {
            this.Podcast = podcast;
        }

        public bool Equals(Episode other)
        {
            return this.Guid == other.Guid;
        }
    }

    public static class Extensions
    {
        public static DateTime ToDateTime(this string input)
        {
            var index = input.LastIndexOf(" ");
            var stringResult = input
                .Remove(index).Replace("Thurs", "Thu")
                .Replace("Sept", "Sep")
                .Replace("Fri, 15 Jan 2015", "Fri, 15 Jan 2016");

            var dateResult = DateTime.MinValue;
            var parsed = DateTime.TryParse(stringResult, out dateResult);

            return dateResult;
        }
    }
}