using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePodcast = RobertIagar.Podcasts.Core.Entities.Podcast;
using CoreEpisode = RobertIagar.Podcasts.Core.Entities.Episode;
using RobertIagar.Podcasts.UWP.ViewModels;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static PodcastViewModel ToPodcastViewModel(this CorePodcast podcast)
        {
            var podcastModel = new Podcast()
            {
                Author = podcast.Author,
                DateAdded = podcast.DateAdded,
                Description = podcast.Description,
                FeedUrl = podcast.FeedUrl,
                ImageUrl = podcast.ImageUrl,
                Title = podcast.Title,
                Core = podcast
            };
            podcast.Episodes.ForEach(e => podcastModel.Episodes.Add(e.ToEpisodeModel(podcastModel)));

            return new PodcastViewModel(podcastModel);
        }

        public static Episode ToEpisodeModel(this CoreEpisode episode, Podcast podcast)
        {
            var episodeModel = new Episode
            {
                Podcast = podcast,
                Author = episode.Author,
                Guid = episode.Guid,
                ImageUrl = episode.ImageUrl,
                Title = episode.Title,
                Subtitle = episode.Subtitle,
                Path = episode.Path,
                Published = episode.Published,
                Summary = episode.Summary,
                Core = episode
            };

            return episodeModel;
        }
    }
}
