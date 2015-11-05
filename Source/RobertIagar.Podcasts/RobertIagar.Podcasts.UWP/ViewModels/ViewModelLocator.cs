using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RobertIagar.Podcasts.Core.Entities;
using RobertIagar.Podcasts.Core.Interfaces;
using RobertIagar.Podcasts.Services;
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

            SimpleIoc.Default.Register<IPodcastService, PodcastService>();
            SimpleIoc.Default.Register<IFileDownloadService, FileDownloadService>();
            SimpleIoc.Default.Register<IFeedParaseService, FeedParserService>();
            SimpleIoc.Default.Register<ILocalStorageService<Podcast>, LocalPodcastService>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}
