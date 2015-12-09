using RobertIagar.Podcasts.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Messages
{
    public class PlayEpisodeMessage
    {
        public PlayEpisodeMessage(Episode episode)
        {
            this.Episode = episode;
        }

        public Episode Episode { get; }
    }
}
