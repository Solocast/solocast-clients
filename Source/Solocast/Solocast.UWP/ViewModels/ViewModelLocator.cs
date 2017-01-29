using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using Solocast.DAL;
using Solocast.Services;
using Solocast.UWP.Infrastructure.Services;
using Solocast.UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.UWP.ViewModels
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			//services
			SimpleIoc.Default.Register<IPodcastService, PodcastService>();
			SimpleIoc.Default.Register<IFileDownloadService, FileDownloadService>();
			SimpleIoc.Default.Register<IFeedParaseService, XmlFeedParserService>();
			SimpleIoc.Default.Register<IMessageDialogService, MessageDialogService>();
			SimpleIoc.Default.Register<IPodcastStore<Podcast>>(() => new SQLitePodcastService());
			SimpleIoc.Default.Register<IDatabaseMigrator>(() => new SQLitePodcastService());
			SimpleIoc.Default.Register<IPlayService, PlayService>();
			SimpleIoc.Default.Register<ISearchService, ItunesSearchService>();

			//navigation service
			SimpleIoc.Default.Register<INavigationService>(() =>
			{
				var navigationService = new AppShellNavigationService();
				//TODO: add more pages
				navigationService.Configure(nameof(PodcastsViewModel), typeof(PodcastsPage));
				navigationService.Configure(nameof(PodcastDetailsViewModel), typeof(PodcastDetailsPage));
				navigationService.Configure(nameof(EpisodesViewModel), typeof(EpisodesPage));
				navigationService.Configure(nameof(NowPlayingViewModel), typeof(NowPlayingPage));
				navigationService.Configure(nameof(SearchViewModel), typeof(SearchPage));
				return navigationService;
			});

			//view models
			SimpleIoc.Default.Register<NowPlayingViewModel>(true);
			SimpleIoc.Default.Register<PodcastsViewModel>();
			SimpleIoc.Default.Register<PodcastDetailsViewModel>();
			SimpleIoc.Default.Register<EpisodesViewModel>();
			SimpleIoc.Default.Register<SearchViewModel>();
		}

		public PodcastsViewModel Podcasts => SimpleIoc.Default.GetInstance<PodcastsViewModel>();

		public PodcastDetailsViewModel PodcastDetails => SimpleIoc.Default.GetInstance<PodcastDetailsViewModel>();

		public NowPlayingViewModel NowPlaying => SimpleIoc.Default.GetInstance<NowPlayingViewModel>();

		public EpisodesViewModel Episodes => SimpleIoc.Default.GetInstance<EpisodesViewModel>();

		public SearchViewModel Search => SimpleIoc.Default.GetInstance<SearchViewModel>();
	}
}
