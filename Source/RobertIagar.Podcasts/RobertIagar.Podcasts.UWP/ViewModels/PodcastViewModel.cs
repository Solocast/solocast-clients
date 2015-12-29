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
using System.Collections.ObjectModel;
using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using RobertIagar.Podcasts.Core.Contracts;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class PodcastViewModel : ViewModelBase, IEquatable<PodcastViewModel>
    {
        private ObservableCollection<EpisodeViewModel> episodes;

        public PodcastViewModel(Podcast podcast)
        {
            Podcast = podcast;
            DeletePodcastCommand = new RelayCommand(DeletePodcast);
            PlayPodcastCommand = new RelayCommand(PlayPodcast, CanPlayPodcast);
            podcast.Episodes.ForEach(e => e.Podcast = podcast);
            this.episodes = new ObservableCollection<EpisodeViewModel>(podcast.Episodes.Select(
                ep =>
                new EpisodeViewModel(SimpleIoc.Default.GetInstance<IFileDownloadService>(),
                SimpleIoc.Default.GetInstance<IMessageDialogService>(),
                ep)
                ));
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
