using Newtonsoft.Json;
using RobertIagar.Podcasts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using RobertIagar.Podcasts.Core.Entities;

namespace RobertIagar.Podcasts.Services
{
    public class FeedParser : IFeedParser
    {
        HttpClient client;

        public FeedParser()
        {
            client = new HttpClient();
        }

        public async Task<dynamic> GetFeedAsync(Uri feedUrl)
        {
            var xml = await client.GetStringAsync(feedUrl);
            var xDoc = XDocument.Parse(xml);

            string json = JsonConvert.SerializeXNode(xDoc);
            dynamic result = JsonConvert.DeserializeObject(json);
            return result;
        }

        public Podcast GetPodcast(dynamic json)
        {
            string link = json.rss.channel.link.ToString();
            string title = json.rss.channel.title.ToString();
            string author = json.rss.channel["itunes:author"].ToString();
            string summary = json.rss.channel["itunes:summary"].ToString();
            string description = json.rss.channel.description.ToString();
            string imageLink = json.rss.channel.image.url ?? json.rss.channel["itunes:image"].href.ToString();
            string podcastDescription = string.Empty;

            if (description == null)
                podcastDescription = summary;
            else if (summary == null)
                podcastDescription = summary;

            return new Podcast(title, description, author, new Uri(link), new Uri(imageLink), DateTime.Now);
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
