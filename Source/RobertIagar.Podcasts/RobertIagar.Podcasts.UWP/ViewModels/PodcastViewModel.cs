using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using RobertIagar.Podcasts.UWP.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class PodcastViewModel : ViewModelBase, IEquatable<PodcastViewModel>
    {
        public PodcastViewModel(Podcast podcast)
        {
            Podcast = podcast;
            DeletePodcastCommand = new RelayCommand(DeletePodcast);
            PlayPodcastCommand = new RelayCommand(PlayPodcast, CanPlayPodcast);
        }

        public Podcast Podcast { get; private set; }
        public ICommand DeletePodcastCommand { get; private set; }
        public ICommand PlayPodcastCommand { get; private set; }

        public bool Equals(PodcastViewModel other)
        {
            return Podcast.Equals(other.Podcast);
        }

        private void DeletePodcast()
        {
            MessengerInstance.Send(new DeletePodcastMessage(this));
        }

        private void PlayPodcast()
        {
            throw new NotImplementedException();
        }

        private bool CanPlayPodcast()
        {
            try
            {
                PlayPodcast();
            }
            catch (NotImplementedException)
            {
                return false;
            }

            return true;
        }
    }
}
