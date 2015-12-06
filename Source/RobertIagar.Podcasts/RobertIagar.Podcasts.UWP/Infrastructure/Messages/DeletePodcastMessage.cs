using RobertIagar.Podcasts.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Messages
{
    public class DeletePodcastMessage
    {
        public DeletePodcastMessage(PodcastViewModel podcast)
        {
            this.PodcastViewModel = podcast;
        }

        public PodcastViewModel PodcastViewModel { get; private set; }
    }
}
