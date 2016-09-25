using Newtonsoft.Json;
using Solocast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Solocast.Core.Contracts;
using Solocast.Services.Extensions;

namespace Solocast.Services
{
    public class FeedParserService : IFeedParaseService
    {
        HttpClient client;

        public FeedParserService()
        {
            client = new HttpClient();
        }

        private async Task<dynamic> GetChannelNodeAsync(string feedUrl)
        {
            return await GetChannelNodeAsync(new Uri(feedUrl));
        }

        private async Task<dynamic> GetChannelNodeAsync(Uri feedUrl)
        {
            var xml = await client.GetStringAsync(feedUrl);
            var xDoc = XDocument.Parse(xml);

            string json = JsonConvert.SerializeXNode(xDoc);
            dynamic result = JsonConvert.DeserializeObject(json);
            return result.rss.channel;
        }

        public async Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            var json = await GetChannelNodeAsync(feedUrl);

            string link = feedUrl;
            string title = json.title.ToString();
            string author = json["itunes:author"]?.ToString();
            string summary = json["itunes:summary"]?.ToString();
            string description = json.description?.ToString();
            string imageLink = json.image.url ?? json["itunes:image"]["@href"].ToString();
            string podcastDescription = string.Empty;

            if (description == null)
                podcastDescription = summary;
            else if (summary == null)
                podcastDescription = description;
            else
                podcastDescription = description;

            podcastDescription = podcastDescription.RemoveCData();

            var podcast = new Podcast(title, podcastDescription, author, link, imageLink, DateTime.Now);

            var episodes = new List<Episode>();

            var items = json.item;

            foreach (var item in items)
            {
                title = item.title.ToString();
                string subtitle = item["itunes:subtitle"].ToString();
                string path = item.enclosure["@url"].ToString();
                author = item["itunes:author"].ToString();
                summary = item["itunes:summary"].ToString();
                string pubDate = item.pubDate.ToString();
                string imageUrl = item["itunes:image"] != null ? item["itunes:image"]["@href"] : imageLink;
                string guid = item.guid.ToString();

                var episode = new Episode(title, subtitle, path, author, summary, pubDate, imageUrl, guid);
                episode.SetPodcast(podcast);
                episodes.Add(episode);
            }

            podcast.SetEpisodes(episodes.OrderByDescending(e=>e.Published));
            return podcast;
        }
    }
}
/*
+		json.rss.channel.link	{http://www.monstercat.com/podcast/}	dynamic {Newtonsoft.Json.Linq.JValue}
+		json.rss.channel.title	{Monstercat Podcast}	dynamic {Newtonsoft.Json.Linq.JValue}
+		json.rss.channel["itunes:author"]	{Monstercat}	dynamic {Newtonsoft.Json.Linq.JValue}
+		json.rss.channel["itunes:summary"]	{The Monstercat Podcast is a weekly radio show featuring a continuous mix of dance music. Perfect to get you hyped for the weekend, it features music from artists such as Krewella, Pegboard Nerds, Lets be Friends, Project 46 and more!}	dynamic {Newtonsoft.Json.Linq.JValue}
+		json.rss.channel.description	{The Monstercat Podcast is a weekly radio show featuring a continuous mix of dance music. Perfect to get you hyped for the weekend, it features music from artists such as Krewella, Pegboard Nerds, Haywyre, Lets be Friends, Project 46 and more!}	dynamic {Newtonsoft.Json.Linq.JValue}

*/
