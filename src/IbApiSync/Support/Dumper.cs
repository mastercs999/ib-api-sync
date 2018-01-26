using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    public static class Dumper
    {
        private static readonly string nullString = "<NULL>";
        private static readonly string emptyStringString = "<EMPTY_STR>";
        private static readonly string emptyListString = "<EMPTY_LIST>";
        private static readonly string listString = "<LIST>";
        private static readonly int PadLength = 30;
        private static readonly int LevelPad = 4;

        public static string Dump(this object obj)
        {
            return obj.Dump(0);
        }

        public static string Pad(this string str)
        {
            return str.PadRight(PadLength);
        }



        private static string Dump(this object obj, int level)
        {
            if (obj == null)
                return nullString;
            else if (obj is bool || obj is int || obj is double || obj is decimal || obj is long || obj is short || obj is ulong)
                return obj.ToString();
            else if (obj is DateTime)
                return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.ffff");
            else if (obj is DateTimeOffset)
                return ((DateTimeOffset)obj).ToString("yyyy-MM-dd HH:mm:ss.ffff");
            else if (obj is string)
            {
                if ((obj as string) == String.Empty)
                    return emptyStringString;
                else
                    return obj as string;
            }
            else if (IsList(obj))
            {
                IList list = obj as IList;

                if (list.Count == 0)
                    return emptyListString;
                else
                    return String.Join(Environment.NewLine, (new object[] { listString }).Concat((list as IEnumerable<object>).Select(x => x.Dump(level) + Environment.NewLine)));
            }
            else
                return String.Join(Environment.NewLine, obj.GetType().GetProperties().OrderBy(x => x.Name).Select(x => (x.Name + ":").Pad().ApplyOffset(level) + x.GetValue(obj).Dump(level + 1)));
        }

        private static string ApplyOffset(this string str, int level)
        {
            return new string(' ', level * LevelPad) + str;
        }

        private static bool IsList(object obj)
        {
            if (obj == null)
                return false;

            return obj is IList &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
