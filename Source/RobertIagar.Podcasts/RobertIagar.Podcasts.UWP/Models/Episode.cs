using GalaSoft.MvvmLight;
using System;
using RobertIagar.Podcasts.Core.Entities;
using Windows.Networking.BackgroundTransfer;

namespace RobertIagar.Podcasts.UWP.Models
{
    public class Episode : ObservableObject
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Guid { get; set; }
        public DateTime Published { get; set; }
        public Uri ImageUrl { get; set; }

        public virtual Podcast Podcast { get; set; }
        public Core.Entities.Episode Core { get; internal set; }
    }
}