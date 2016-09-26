using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Solocast.Services;
using System.Diagnostics;
using Solocast.Core.Contracts;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace Solocast.Tests
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
            var podcast = feedParser.GetPodcastAsync("http://monstercat.com/podcast/feed.xml").Result;

            Assert.AreEqual(true, true);
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
