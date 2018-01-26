using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    public static class Utils
    {
        public static object ParseValue(Type targetType, string rawValue)
        {
            if (targetType == typeof(string))
                return rawValue;
            else if (targetType == typeof(int))
                return int.Parse(rawValue);
            else if (targetType == typeof(double))
                return double.Parse(rawValue);
            else if (targetType == typeof(decimal))
                return decimal.Parse(rawValue);
            else if (targetType == typeof(bool))
                return bool.Parse(rawValue);
            else
                throw new ArgumentException($"Can't parse unknown target type {targetType.Name} from raw value '{rawValue}'");
        }

        public static WebClient PrepareBrowserWebClient()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0");
                wc.Headers.Add("Accept", "text/html");

                return wc;
            }
        }

        public static T[] GetEnumArray<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
