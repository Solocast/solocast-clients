using RobertIagar.Podcasts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobertIagar.Podcasts.Core.Entities;
using Windows.Storage;
using System.Diagnostics;

namespace RobertIagar.Podcasts.Services
{
    public class PodcastService : IPodcastService
    {
        private IFeedParaseService feedParser;
        private ILocalPodcastService storageService;
        private IFileDownloadService fileDownloadManager;

        public PodcastService(IFeedParaseService feedParser, ILocalPodcastService storageService, IFileDownloadService fileDownloadManager)
        {
            this.feedParser = feedParser;
            this.storageService = storageService;
            this.fileDownloadManager = fileDownloadManager;
        }

        public async Task<Podcast> GetPodcastAsync(string feedUrl)
        {
            dynamic feed = await feedParser.GetChannelNodeAsync(feedUrl);
            var podcast = feedParser.GetPodcast(feed);
            return podcast;
        }

        public async Task<IEnumerable<Podcast>> GetPodcastsAsync()
        {
            var podcasts = await storageService.LoadAsync();
            return podcasts;
        }

        public async Task<IEnumerable<Episode>> GetNewEpisodesAsync(Podcast podcast)
        {
            dynamic feed = await feedParser.GetChannelNodeAsync(podcast.FeedUrl);
            Podcast newPodcast = feedParser.GetPodcast(feed);
            var newEpisodes = new List<Episode>();

            foreach (var episode in newPodcast.Episodes)
            {
                if (!podcast.Episodes.Contains(episode))
                {
                    newEpisodes.Add(episode);
                }
            }

            return newEpisodes;
        }

        public async Task SavePodcastAsync(Podcast podcast)
        {
            await storageService.SaveAsync(podcast);
        }

        public async Task SavePodcastsAsync(IEnumerable<Podcast> podcasts)
        {
            await storageService.SaveAsync(podcasts);
        }

        public async Task DownloadEpisodeAsync(Episode episode)
        {
            var file = await fileDownloadManager.DownloadFileAsync(
                appFolderName: AppName,
                folderName: episode.Podcast.Title,
                fileName: string.Format("{0:dd.mm.yyyy} - {1} - {2}", episode.Published, episode.Author, episode.Name),
                fileUrl: episode.Path,
                callback: c =>
                {
                    var bytesReceived = c.Progress.BytesReceived;
                    var bytesToReceive = c.Progress.TotalBytesToReceive;
                    double percent = (bytesReceived * 100) / bytesToReceive;

                    Debug.WriteLine(string.Format("Received: {0}/{1} ({2:P1})", bytesReceived, bytesToReceive, percent / 100.0));
                },
                errorCallback: ex =>
                {
                    Debug.WriteLine(string.Format(ex.Message));
                }
                );

            if (file != null)
            {
                var podcast = episode.Podcast;
                var episodeToUpdate = podcast.Episodes.Where(e => e.Guid == episode.Guid).SingleOrDefault();

                episodeToUpdate.Path = file.Path;
                await storageService.SaveAsync(episode.Podcast);
            }
        }

        public Task<IEnumerable<Podcast>> SearchPodcast(string searchString)
        {
            throw new NotImplementedException();
        }

        public string AppName
        {
            get { return "Solocast"; }
        }
    }
}
