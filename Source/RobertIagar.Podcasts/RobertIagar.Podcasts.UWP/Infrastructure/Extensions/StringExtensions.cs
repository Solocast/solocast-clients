using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static Uri ToUri(this string uriString)
        {
            return new Uri(uriString);
        }

        public static bool IsLocalPath(this string pathString)
        {
            if (pathString.StartsWith("http://") ||
                pathString.StartsWith("https://"))
                return false;

            return true;
        }
    }
}
