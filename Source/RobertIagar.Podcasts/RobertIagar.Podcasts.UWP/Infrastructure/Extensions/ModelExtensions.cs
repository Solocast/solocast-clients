using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePodcast = RobertIagar.Podcasts.Core.Entities.Podcast;
using CoreEpisode = RobertIagar.Podcasts.Core.Entities.Episode;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static Podcast ToPodcastModel(this CorePodcast podcast)
        {
            return new Podcast
            {
                Author = podcast.Author,
                DateAdded = podcast.DateAdded,
                Description = podcast.Description,
                FeedUrl = podcast.FeedUrl,
                ImageUrl = podcast.ImageUrl,
                Title = podcast.Title
            };
        }

        public static Episode ToEpisodeModel(this CoreEpisode episode)
        {
            return new Episode
            {
                Author = episode.Author,
                Guid = episode.Guid,
                ImageUrl = episode.ImageUrl,
                Name = episode.Name,
                Path = episode.Path,
                Published = episode.Published,
                Summary = episode.Summary
            };
        }
    }
}
