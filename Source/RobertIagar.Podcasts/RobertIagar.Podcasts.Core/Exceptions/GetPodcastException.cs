using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Exceptions
{
    public class GetPodcastException : Exception
    {
        public GetPodcastException(string podcastLink, Exception innerException)
            :base(string.Format("Cannot load podcast link: {0}",podcastLink), innerException)
        {
            this.PodcastLink = podcastLink;
        }

        public string PodcastLink { get; private set; }
    }
}
