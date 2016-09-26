using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.UWP.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static Uri ToUri(this string uriString)
        {
            return new Uri(uriString);
        }
    }
}
