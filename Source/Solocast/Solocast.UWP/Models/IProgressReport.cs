using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace Solocast.UWP.Models
{
    public interface IProgressReport
    {
        void ReportProgress(DownloadOperation operation);
    }
}
