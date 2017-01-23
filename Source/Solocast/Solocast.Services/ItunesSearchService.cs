using Solocast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solocast.Core.Contracts;
using Newtonsoft.Json;
using Windows.Web.Http;

namespace Solocast.Services
{
	public class ItunesSearchService : ISearchService
	{
		private HttpClient client;

		public ItunesSearchService()
		{
			client = new HttpClient();
		}

		public async Task<IEnumerable<SearchPodcast>> SearchPodcastAsync(string podcastName)
		{
			var term = $"term={podcastName.Replace(" ", "+")}&entity=podcast";
			var searchResult = await client.GetStringAsync(new Uri("https://itunes.apple.com/search?" + term));
			dynamic jsonResult = JsonConvert.DeserializeObject(searchResult);
			var list = new List<SearchPodcast>();
			foreach (dynamic item in jsonResult.results)
			{
				if (item.kind == "podcast")
				{
					var podcast = new SearchPodcast
					{
						Name = item.trackName,
						FeedUrl = new Uri(item.feedUrl?.ToString()),
						ArtworkUrl30 = new Uri(item.artworkUrl30?.ToString()),
						ArtworkUrl60 = new Uri(item.artworkUrl60?.ToString()),
						ArtworkUrl100 = new Uri(item.artworkUrl100?.ToString()),
						ArtworkUrl600 = new Uri(item.artworkUrl600?.ToString())
					};

					list.Add(podcast);
				}
			}

			return list;
		}
	}
}
