using Solocast.Core.Contracts;

namespace Solocast.UWP.Infrastructure.Messages
{
	public class SubcribeToPodcastMessage
	{
		public Podcast Podcast { get; }

		public SubcribeToPodcastMessage(Podcast podcast)
		{
			Podcast = podcast;
		}
	}
}