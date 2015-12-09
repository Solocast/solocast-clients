using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Extensions
{
    public class ImageExtensions
    {
        public static Uri GetCacheUri(DependencyObject obj)
        {
            return (Uri)obj.GetValue(CacheUriProperty);
        }

        public static void SetCacheUri(DependencyObject obj, Uri value)
        {
            obj.SetValue(CacheUriProperty, value);
        }

        // Using a DependencyProperty as the backing store for CacheUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CacheUriProperty =
            DependencyProperty.RegisterAttached("CacheUri", typeof(Uri), typeof(ImageExtensions), new PropertyMetadata(null, OnCacheChanged));

        private static async void OnCacheChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Uri newCacheUri = (Uri)d.GetValue(CacheUriProperty);

            if (newCacheUri != null)
            {
                //cache image to local storage
                await Task.Delay(0);

            }
            else
            {
                SetSourceOnObject(d, null, false);
            }

        }

        private static void SetSourceOnObject(object imgControl, ImageSource imageSource, bool throwEx = true)
        {

            try
            {
                if (imgControl is Image)
                {
                    ((Image)imgControl).Source = imageSource;
                }
                else
                {
                    if (imgControl is ImageBrush)
                    {
                        ((ImageBrush)imgControl).ImageSource = imageSource;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (throwEx)
                {
                    throw ex;
                }
            }

        }
    }
}
