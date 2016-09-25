using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solocast.Services.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveCData(this string input)
        {
            var regex = new Regex("\"(.*?)\"");
            var matches = regex.Matches(input);
            if (matches.Count == 2)
            {
                return matches[1].Value.Replace("\"", "");
            }

            return input;
        }

        public static string GetExtension(this string input)
        {
            var index = input.LastIndexOf('.');
            return input.Substring(index);
        }

        public static string RemoveIllegalPathChars(this string input)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(input, "");
        }
    }
}
