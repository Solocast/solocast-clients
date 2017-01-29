using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solocast.UWP.ViewModels
{
	public class SearchViewModel : ViewModelBase
	{
		ISearchService searchService;
		ObservableCollection<SearchPodcast> podcasts;
		string searchTerms;

		public SearchViewModel(ISearchService searchService)
		{
			this.searchService = searchService;
			this.podcasts = new ObservableCollection<SearchPodcast>();
			this.SearchCommand = new RelayCommand(async () => await this.SearchPodcastAsync(), CanSearchPodcast);
		}

		public string SearchTerms
		{
			get { return searchTerms; }
			set
			{
				Set(nameof(SearchTerms), ref searchTerms, value);
				((RelayCommand)SearchCommand).RaiseCanExecuteChanged();
			}
		}

		public IList<SearchPodcast> Podcasts => podcasts;

		private async Task SearchPodcastAsync()
		{
			if (!string.IsNullOrEmpty(searchTerms))
			{
				var searchResults = await searchService.SearchPodcastAsync(searchTerms);
				podcasts.Clear();
				foreach (var item in searchResults)
				{
					podcasts.Add(item);
				}
			}
		}

		private bool CanSearchPodcast()
		{
			return !string.IsNullOrEmpty(SearchTerms);
		}

		public ICommand SearchCommand { get; }
	}
}
