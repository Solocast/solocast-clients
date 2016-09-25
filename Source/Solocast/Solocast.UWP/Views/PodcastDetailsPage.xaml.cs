using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Solocast.UWP.Models;
using Solocast.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Solocast.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PodcastDetailsPage : Page
    {
        private CanvasBitmap bitmap;
        private ICanvasImage effect;
        private Vector2 currentEffectSize;

        public PodcastDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as PodcastViewModel;
            if (this.DataContext != null)
            {
                (this.DataContext as PodcastDetailsViewModel).Podcast = parameter;
            }

            base.OnNavigatedTo(e);
        }

        #region Useless code but keeping it for the sake of the Win2D Effects
        public void CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            bitmap = await CanvasBitmap.LoadAsync(sender, (this.DataContext as PodcastDetailsViewModel).Podcast.Podcast.ImageUrl);

            effect = CreateEffect();
        }

        public void Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var size = sender.Size;
            using (var ds = args.DrawingSession)
            {
                ds.DrawImage(effect, (size.ToVector2() - currentEffectSize) / 2);
                var brush = new CanvasImageBrush(sender, this.effect)
                {
                    ExtendX = CanvasEdgeBehavior.Wrap,
                    ExtendY = CanvasEdgeBehavior.Wrap,
                    SourceRectangle = new Rect(0, bitmap.SizeInPixels.Height - 96, 96, 96)
                };

                ds.FillRectangle(0, 0, (float)this.ActualWidth, (float)this.ActualHeight, brush);
            }
            sender.Invalidate();
        }

        public void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateEffect();
        }

        ICanvasImage CreateEffect()
        {
            var blurEffect = new GaussianBlurEffect
            {
                Source = bitmap
            };

            if (bitmap != null)
                currentEffectSize = bitmap.Size.ToVector2();

            blurEffect.BlurAmount = 10f;
            return blurEffect;
        }
        #endregion
    }
}
