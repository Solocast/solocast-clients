using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobertIagar.Podcasts.UWP.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class PodcastDetailsViewModel : ViewModelBase
    {
        private Podcast podcast;

        public PodcastDetailsViewModel()
        {
            if (IsInDesignMode)
            {
                Podcast = new Podcast
                {
                    Author = "Monstercat",
                    DateAdded = DateTime.UtcNow,
                    Description = "Monstercat Podcast Description",
                    FeedUrl = new Uri("https://monstercat.com/podcast/feed.xml"),
                    Title = "Monstercat Podcast",
                    ImageUrl = new Uri("http://www.monstercat.com/podcast/itunes_cover.jpg"),

                };
                Podcast.Episodes.Add(new Episode
                {
                    Author = "Monstercat",
                    Guid = "jfkajfkasd",
                    ImageUrl = new Uri("http://www.monstercat.com/podcast/81/cover.jpg"),
                    Name = "Monstercat Podcast Ep. 081",
                    Path = "path",
                    Podcast = Podcast,
                    Published = DateTime.UtcNow.AddDays(-2),
                    Summary = "summary"
                });
                Podcast.Episodes.Add(new Episode
                {
                    Author = "Monstercat",
                    Guid = "jfkajfkasd",
                    ImageUrl = new Uri("http://www.monstercat.com/podcast/80/cover.jpg"),
                    Name = "Monstercat Podcast Ep. 080",
                    Path = "path",
                    Podcast = Podcast,
                    Published = DateTime.UtcNow.AddDays(-3),
                    Summary = "summary"
                });
            }
        }


        public Podcast Podcast
        {
            get { return podcast; }
            set { Set(nameof(Podcast), ref podcast, value); }
        }

       
    }
}
