using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solocast.UWP.Models;
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
using Solocast.UWP.Infrastructure.Extensions;
using Solocast.UWP.Infrastructure.Services;
using Solocast.UWP.Infrastructure.Messages;

namespace Solocast.UWP.ViewModels
{
    public class PodcastDetailsViewModel : ViewModelBase
    {
        private IPlayService playService;
        private PodcastViewModel podcast;

        public PodcastDetailsViewModel(IPlayService nowPlayingService)
        {
            this.playService = nowPlayingService;
        }


        public PodcastViewModel Podcast
        {
            get { return podcast; }
            set { Set(nameof(Podcast), ref podcast, value); }
        }

        public void PlayEpisode(object sender, ItemClickEventArgs parameters)
        {
            var episodeViewModel = parameters.ClickedItem as EpisodeViewModel;

            //var content = new ToastContent()
            //{
            //    Launch = null,
            //    Visual = new ToastVisual()
            //    {
            //        TitleText = new ToastText()
            //        {
            //            Text = $"Listening to {episode.Title}"
            //        },
            //        BodyTextLine1 = new ToastText()
            //        {
            //            Text = $"By {episode.Author}"
            //        },
            //        AppLogoOverride = new ToastAppLogo()
            //        {
            //            Crop = ToastImageCrop.None,
            //            Source = new ToastImageSource(episode.ImageUrl.OriginalString)
            //        },
            //    }
            //};

            //var t = content.GetContent();

            //var toast = new ToastNotification(content.GetXml());

            //ToastNotificationManager.CreateToastNotifier().Show(toast);

            MessengerInstance.Send(new PlayEpisodeMessage(episodeViewModel.Episode));
        }
    }
}
