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
    /// <summary>
    /// Class containing methods useful among different scopes in an application.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// This method gets type and its value in string. It converts this string value into a value of target type. So "12" converts to int for example.
        /// </summary>
        /// <param name="targetType">Type in which the given value should be converted.</param>
        /// <param name="rawValue">Value in pure string/</param>
        /// <returns>Converted string value into target tyoe.</returns>
        /// <exception cref="ArgumentException">Throw when target type is not supported.</exception>
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

        /// <summary>
        /// Creates WebClients which acts like a browser. It mean User-Agent header and Acceot header are set.
        /// </summary>
        /// <returns>WebClient instance with User=Agent and Accept header.</returns>
        public static WebClient PrepareBrowserWebClient()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0");
                wc.Headers.Add("Accept", "text/html");

                return wc;
            }
        }

        /// <summary>
        /// Creates an array which contains all values of given enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum whose values we want.</typeparam>
        /// <returns>Array of all enum values.</returns>
        public static T[] GetEnumArray<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
