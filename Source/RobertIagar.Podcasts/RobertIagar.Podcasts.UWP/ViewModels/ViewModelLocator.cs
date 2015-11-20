using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using RobertIagar.Podcasts.Core.Entities;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Services;
using RobertIagar.Podcasts.UWP.Infrastructure.Services;
using RobertIagar.Podcasts.UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //services
            SimpleIoc.Default.Register<IPodcastService, PodcastService>();
            SimpleIoc.Default.Register<IFileDownloadService, FileDownloadService>();
            SimpleIoc.Default.Register<IFeedParaseService, FeedParserService>();
            SimpleIoc.Default.Register<ILocalPodcastService>(() =>
            {
                return new LocalPodcastService("podcasts.json");
            });

            //navigation service
            SimpleIoc.Default.Register<INavigationService>(() =>
            {
                var navigationService = new AppShellNavigationService();
                //TODO: add more pages
                navigationService.Configure(nameof(PodcastsViewModel), typeof(PodcastsPage));
                navigationService.Configure(nameof(PodcastDetailsViewModel), typeof(PodcastDetailsPage));
                return navigationService;
            });


            //view models
            SimpleIoc.Default.Register<PodcastsViewModel>();
            SimpleIoc.Default.Register<PodcastDetailsViewModel>();
        }

        public PodcastsViewModel Podcasts
        {
            get { return ServiceLocator.Current.GetInstance<PodcastsViewModel>(); }
        }

        public PodcastDetailsViewModel PodcastDetails
        {
            get { return ServiceLocator.Current.GetInstance<PodcastDetailsViewModel>(); }
        }
    }
}
