using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using RobertIagar.Podcasts.Services;
using System.Diagnostics;
using RobertIagar.Podcasts.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace RobertIagar.Podcasts.Tests
{
    /// <summary>
    /// This class is used for testing purposes only. No actual unit testing is done. For now...
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RandomTest()
        {
            var feedParser = new FeedParserService();
            dynamic json = feedParser.GetChannelNodeAsync("http://monstercat.com/podcast/feed.xml").Result;
            var podcast = feedParser.GetPodcast(json, "http://monstercat.com/podcast/feed.xml");

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void TestFeedParser()
        {
            var feedParser = new FeedParserService();
            dynamic channel = feedParser.GetChannelNodeAsync("http://monstercat.com/podcast/feed.xml").Result;
            string podcastDescription = string.Empty;
            string summary = channel["itunes:summary"].ToString();
            string description = channel.description.ToString();

            if (description == null)
                podcastDescription = summary;
            else if (summary == null)
                podcastDescription = description;


            Podcast podcast = feedParser.GetPodcast(channel, "http://monstercat.com/podcast/feed.xml");

            Assert.AreEqual(channel.title.ToString(), podcast.Title);
            Assert.AreEqual(new Uri(channel.image.url.ToString()), podcast.ImageUrl);
            Assert.AreEqual(podcastDescription, podcast.Description);
            Assert.AreEqual(channel["itunes:author"].ToString(), podcast.Author);
            Assert.AreEqual(new Uri(channel.link.ToString()), podcast.FeedUrl);
        }

        [TestMethod]
        public void TestIfPodcastFromFeedIsTheSameAsPodcastFromService()
        {
            var feedParser = new FeedParserService();

            var podcastService = new PodcastService(feedParser, null, null);
            var feed = feedParser.GetChannelNodeAsync("http://monstercat.com/podcast/feed.xml").Result;

            var podcastFromFeed = feedParser.GetPodcast(feed, "http://monstercat.com/podcast/feed.xml");
            var podcastFromService = podcastService.GetPodcastAsync("http://monstercat.com/podcast/feed.xml").Result;

            Assert.AreEqual(true, podcastFromService.Equals(podcastFromFeed));
        }

        [TestMethod]
        public void TestLocalStorage()
        {
            var localStorage = new LocalPodcastService("podcasts.json");
            var feedParser = new FeedParserService();
            var podcastService = new PodcastService(feedParser, localStorage, null);

            var podcasts = new List<Podcast>();
            var podcast = podcastService.GetPodcastAsync("http://monstercat.com/podcast/feed.xml").Result;
            podcasts.Add(podcast);

            podcastService.SavePodcastsAsync(podcasts).Wait();

            var podcastsFromStorage = podcastService.GetPodcastsAsync().Result.ToList();
            Assert.AreEqual(podcasts.Count, podcastsFromStorage.Count);

            for (int i = 0; i < podcasts.Count; i++)
            {
                Assert.AreEqual(true, podcasts[i].Equals(podcastsFromStorage[i]));
            }
        }

        [TestMethod]
        public void TestDownload()
        {
            var localStorage = new LocalPodcastService("podcasts.json");
            var feedParser = new FeedParserService();
            var fileManager = new FileDownloadService();
            var podcastService = new PodcastService(feedParser, localStorage, fileManager);

            var podcastFromService = podcastService.GetPodcastAsync("http://monstercat.com/podcast/feed.xml").Result;
            podcastService.DownloadEpisodeAsync(podcastFromService.Episodes.OrderByDescending(e => e.Published).ToList()[0]).Wait();
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = localFolder.GetFileAsync("podcast.json").AsTask().Result;
                file.DeleteAsync().AsTask().Wait();
            }
            catch
            {
                //file does not exist which is ok;
            }
        }
    }
}
