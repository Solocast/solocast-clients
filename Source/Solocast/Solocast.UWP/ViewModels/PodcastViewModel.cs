using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Solocast.UWP.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Solocast.UWP.Infrastructure.Messages;
using System.Collections.ObjectModel;
using Solocast.UWP.Infrastructure.Extensions;
using Solocast.Core.Contracts;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Solocast.Core.Interfaces;
using Solocast.UWP.Infrastructure.Services;
using System.Collections.Specialized;

namespace Solocast.UWP.ViewModels
{
    public class PodcastViewModel : ViewModelBase, IEquatable<PodcastViewModel>
    {
        private ObservableCollection<EpisodeViewModel> episodes;

        public PodcastViewModel(Podcast podcast)
        {
            if (podcast == null)
                throw new ArgumentNullException("podcast");

            Podcast = podcast;
            DeletePodcastCommand = new RelayCommand(DeletePodcast);
            PlayPodcastCommand = new RelayCommand(PlayPodcast, CanPlayPodcast);
            podcast.Episodes.ForEach(e => e.Podcast = podcast);
            this.episodes = new ObservableCollection<EpisodeViewModel>(
                podcast.Episodes.Select(ep => new EpisodeViewModel(ep))
                );
        }

        public Podcast Podcast { get; }
        public ICommand DeletePodcastCommand { get; }
        public ICommand PlayPodcastCommand { get; }

        public bool Equals(PodcastViewModel other)
        {
            return Podcast.Equals(other.Podcast);
        }

        public IList<EpisodeViewModel> Episodes { get { return episodes; } }

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
