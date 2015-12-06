using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using RobertIagar.Podcasts.UWP.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RobertIagar.Podcasts.UWP.Models
{
    public class Podcast : ObservableObject, IEquatable<Podcast>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Uri FeedUrl { get; set; }
        public Uri ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public Core.Entities.Podcast Core { get; set; }

        public Podcast()
        {
            Episodes = new ObservableCollection<Episode>();
        }

        public virtual ObservableCollection<Episode> Episodes { get; private set; }

        public bool Equals(Podcast other)
        {
            return other.FeedUrl == this.FeedUrl;
        }
    }
}
