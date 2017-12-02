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
		public string FeedUrl { get; set; }
		public string ArtworkUrl30 { get; set; }
		public string ArtworkUrl60 { get; set; }
		public string ArtworkUrl100 { get; set; }
		public string ArtworkUrl600 { get; set; }
	}
}
