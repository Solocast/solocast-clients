using GalaSoft.MvvmLight.Messaging;
using Solocast.UWP.Infrastructure.Messages;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Solocast.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PodcastsPage : Page
    {
        public PodcastsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Send(new LoadPodcastsMessage());
            Messenger.Default.Send(new CheckForNewEpsiodesMessage());
        }
    }
}
