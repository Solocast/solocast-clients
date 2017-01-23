using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.Core.Contracts
{
	public class SearchPodcast
	{
		public string Name { get; set; }
		public Uri FeedUrl { get; set; }
		public Uri ArtworkUrl30 { get; set; }
		public Uri ArtworkUrl60 { get; set; }
		public Uri ArtworkUrl100 { get; set; }
		public Uri ArtworkUrl600 { get; set; }
	}
}
