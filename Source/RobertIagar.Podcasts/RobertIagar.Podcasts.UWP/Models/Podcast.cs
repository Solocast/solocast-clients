using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.Models
{
    public class Podcast: ObservableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual ObservableCollection<Episode> Episodes { get; private set; }

        public Podcast()
        {
            Episodes = new ObservableCollection<Episode>();
        }
    }
}
