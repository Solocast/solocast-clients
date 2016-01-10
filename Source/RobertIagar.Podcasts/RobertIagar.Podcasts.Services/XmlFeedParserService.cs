using RobertIagar.Podcasts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobertIagar.Podcasts.Core.Contracts;
using System.Net.Http;
using System.Xml.Linq;
using RobertIagar.Podcasts.Services.Extensions;

namespace RobertIagar.Podcasts.Services
{
    public class XmlFeedParserService : IFeedParaseService
    {
        private HttpClient client;

        public XmlFeedParserService()
        {
            client = new HttpClient();
        }

        public async Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            var xml = await client.GetStringAsync(feedUrl);

            var xmlDocument = XDocument.Parse(xml);

            var title = xmlDocument.Element("rss").Element("channel").Element("title").Value;
            XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
            var items = xmlDocument.Element("rss").Element("channel").Elements("item");
            var description = xmlDocument.Element("rss").Element("channel").Element("description").Value;
            var summary = xmlDocument.Element("rss").Element("channel").Element(itunes + "summary").Value;
            var imageLink = xmlDocument.Element("rss").Element("channel").Element(itunes + "image").Attribute("href").Value;
            var author = xmlDocument.Element("rss").Element("channel").Element(itunes + "author").Value;
            string podcastDescription = string.Empty;

            if (description == null)
                podcastDescription = summary;
            else if (summary == null)
                podcastDescription = description;
            else
                podcastDescription = description;

            podcastDescription = podcastDescription.RemoveCData();

            var podcast = new Podcast(title, podcastDescription, author, feedUrl, imageLink, DateTime.Now);

            var episodes = new List<Episode>();

            foreach (var item in items)
            {
                title = item.Element("title").Value;
                var subtitle = item.Element(itunes + "subtitle").Value;
                var path = item.Element("enclosure").Attribute("url").Value;
                author = item.Element(itunes + "author").Value;
                summary = item.Element(itunes + "summary").Value;
                var pubDate = item.Element("pubDate").Value;
                var imageUrl = item.Element(itunes + "image") != null ? item.Element(itunes + "image").Attribute("href").Value : imageLink;
                var guid = item.Element("guid").Value;

                var episode = new Episode(title, subtitle, path, author, summary, pubDate, imageUrl, guid);
                episode.SetPodcast(podcast);
                episodes.Add(episode);
            }

            podcast.SetEpisodes(episodes);
            return podcast;
        }
    }
}
