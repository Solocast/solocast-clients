using GalaSoft.MvvmLight.Messaging;
using Solocast.UWP.Infrastructure.Messages;
using Solocast.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Solocast.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EpisodesPage : Page
    {
        public EpisodesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Send(new LoadEpisodesMessage());
        }
	}
}
