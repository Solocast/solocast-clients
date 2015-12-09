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
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class PodcastDetailsViewModel : ViewModelBase
    {
        private IPlayService playService;
        private Podcast podcast;

        public PodcastDetailsViewModel(IPlayService nowPlayingService)
        {
            if (IsInDesignMode)
            {
                Podcast = new Podcast()
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
                    Title = "Monstercat Podcast Ep. 081",
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
                    Title = "Monstercat Podcast Ep. 080",
                    Path = "path",
                    Podcast = Podcast,
                    Published = DateTime.UtcNow.AddDays(-3),
                    Summary = "summary"
                });
            }

            this.playService = nowPlayingService;
        }


        public Podcast Podcast
        {
            get { return podcast; }
            set { Set(nameof(Podcast), ref podcast, value); }
        }

        public void PlayEpisode(object sender, ItemClickEventArgs parameters)
        {
            var episode = parameters.ClickedItem as Episode;

            var content = new ToastContent()
            {
                Launch = null,
                Visual = new ToastVisual()
                {
                    TitleText = new ToastText()
                    {
                        Text = $"Listening to {episode.Title}"
                    },
                    BodyTextLine1 = new ToastText()
                    {
                        Text = $"By {episode.Author}"
                    },
                    AppLogoOverride = new ToastAppLogo()
                    {
                        Crop = ToastImageCrop.None,
                        Source = new ToastImageSource(episode.ImageUrl.OriginalString)
                    },
                }
            };

            var t = content.GetContent();

            var toast = new ToastNotification(content.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            MessengerInstance.Send(new PlayEpisodeMessage(episode));
        }
    }
}
