using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using RobertIagar.Podcasts.Services;
using System.Diagnostics;

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
            var feedParser = new FeedParser();
            dynamic json = feedParser.GetFeedAsync(new Uri("http://monstercat.com/podcast/feed.xml")).Result;
            var podcast = feedParser.GetPodcast(json);

            Assert.AreEqual(true, true);
        }
    }
}
