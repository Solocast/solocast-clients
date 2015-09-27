using RobertIagar.Podcasts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface IEpisodeDownloaader
    {
        Task DownloadEpisodeAsync(Episode episode);
    }
}
